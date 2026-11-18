using System.Text.Json;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level7FramebufferDiagnosticTests
{
	[Fact]
	public void CapturesEquivalentPlaybackBoundaryWindowsInBothAtariModes()
	{
		string directory = Path.Combine(
			Path.GetTempPath(), $"tempest-level7-framebuffer-ab-{Guid.NewGuid():N}");
		string playbackPath = Path.GetFullPath("level7-recording.json");
		try
		{
			List<(bool AtariEnabled, long[] RenderFrames)> runFrames = [];
			foreach (bool atariEnabled in new[] { false, true })
			{
				string mode = atariEnabled ? "on" : "off";
				string runtimePath = Path.Combine(directory, $"{mode}.jsonl");
				StringWriter output = new();
				int readbackCount = 0;
				using Level7VisibleRuntimeDiagnostic runtime = new(
					runtimePath,
					"recordedPlayback",
					playbackPath,
					"ExperimentalPortrait",
					60,
					framebufferCaptureEnabled: true,
					atariPresentationEnabled: atariEnabled);
				using Level7FramebufferDiagnostic framebuffer = CreateDiagnostic(
					directory,
					$"{mode}.jsonl",
					output,
					new StringWriter(),
					() =>
					{
						readbackCount++;
						return new Image { Width = 2, Height = 2 };
					},
					(_, path) =>
					{
						File.WriteAllBytes(path, [1, 2, 3]);
						return 3;
					},
					playbackPath,
					atariEnabled);

				CapturePlaybackFrame(runtime, framebuffer, 0, 98);
				long sequence = 1;
				foreach (int boundary in Level7FramebufferDiagnostic.PlaybackCaptureBoundaries)
				{
					CapturePlaybackFrame(runtime, framebuffer, sequence++, boundary);
					if (boundary == 99)
						CapturePlaybackFrame(runtime, framebuffer, sequence++, boundary);
				}
				CapturePlaybackFrame(runtime, framebuffer, sequence, 302);

				Assert.True(framebuffer.IsStopped);
				Assert.Equal(
					Level7FramebufferDiagnostic.MaximumPlaybackFrames,
					framebuffer.CapturedFrameCount);
				Assert.Equal(Level7FramebufferDiagnostic.MaximumPlaybackFrames, readbackCount);
				Assert.True(framebuffer.CapturedBytes <=
					Level7FramebufferDiagnostic.MaximumPlaybackBytes);
				Assert.Equal(120L * 1024 * 1024,
					Level7FramebufferDiagnostic.MaximumPlaybackBytes);
				Assert.Contains("capture complete", output.ToString(), StringComparison.Ordinal);
				runtime.Dispose();

				string[] lines = File.ReadAllLines(runtimePath);
				Assert.Equal(
					Level7FramebufferDiagnostic.MaximumPlaybackFrames,
					lines.Count(line => line.Contains(
						"\"recordType\":\"frame\"", StringComparison.Ordinal)));
				using JsonDocument run = JsonDocument.Parse(lines[0]);
				Assert.Equal(atariEnabled,
					run.RootElement.GetProperty("atariPresentationEnabled").GetBoolean());
				JsonElement selection = run.RootElement.GetProperty("framebufferCaptureSelection");
				Assert.Equal(
					new[] { 100, 150, 200, 250, 300 },
					selection.GetProperty("windowCenters").EnumerateArray()
						.Select(boundary => boundary.GetInt32()));
				Assert.Equal(
					Level7FramebufferDiagnostic.PlaybackFramesPerBoundaryWindow,
					selection.GetProperty("consecutiveFramesPerWindow").GetInt32());
				Assert.Equal("N-1, N, N+1",
					selection.GetProperty("boundaryWindows").GetString());

				JsonDocument[] captures = lines
					.Where(line => line.Contains("\"framebufferCapture\"", StringComparison.Ordinal))
					.Select(line => JsonDocument.Parse(line))
					.ToArray();
				Assert.Equal(Level7FramebufferDiagnostic.MaximumPlaybackFrames, captures.Length);
				Assert.Equal(
					Level7FramebufferDiagnostic.PlaybackCaptureBoundaries,
					captures.Select(capture => capture.RootElement
						.GetProperty("framebufferCapture")
						.GetProperty("playbackBoundary")
						.GetInt32()));
				foreach (JsonDocument document in captures)
				{
					JsonElement frame = document.RootElement;
					JsonElement capture = frame.GetProperty("framebufferCapture");
					Assert.Equal(atariEnabled,
						capture.GetProperty("presentationMode").GetString() == "Atari ON");
					Assert.Equal(playbackPath,
						capture.GetProperty("playbackFilePath").GetString());
					Assert.Equal(frame.GetProperty("playbackBoundary").GetInt64(),
						capture.GetProperty("playbackBoundary").GetInt64());
					Assert.Equal(frame.GetProperty("simulationTick").GetInt64(),
						capture.GetProperty("simulationTick").GetInt64());
					Assert.Equal(7, capture.GetProperty("displayedLevel").GetInt32());
					Assert.Equal("Playing", capture.GetProperty("levelState").GetString());
					Assert.True(File.Exists(Path.Combine(
						Path.Combine(directory, "framebuffer", mode),
						capture.GetProperty("fileName").GetString()!)));
				}
				runFrames.Add((
					atariEnabled,
					captures.Select(capture => capture.RootElement
							.GetProperty("framebufferCapture")
							.GetProperty("renderFrameSequence")
							.GetInt64())
						.ToArray()));
				foreach (JsonDocument capture in captures)
					capture.Dispose();
			}

			Assert.Equal(runFrames[0].RenderFrames, runFrames[1].RenderFrames);
		}
		finally
		{
			if (Directory.Exists(directory))
				Directory.Delete(directory, recursive: true);
		}
	}

	[Fact]
	public void ReportsSkippedPlaybackBoundariesWithoutSubstitutingOtherFrames()
	{
		string directory = Path.Combine(
			Path.GetTempPath(), $"tempest-level7-framebuffer-missed-{Guid.NewGuid():N}");
		string runtimePath = Path.Combine(directory, "runtime.jsonl");
		StringWriter output = new();
		string playbackPath = "level7-recording.json";
		try
		{
			using Level7VisibleRuntimeDiagnostic runtime = new(
				runtimePath,
				"recordedPlayback",
				playbackPath,
				"ExperimentalPortrait",
				60,
				framebufferCaptureEnabled: true);
			using Level7FramebufferDiagnostic framebuffer = CreateDiagnostic(
				directory,
				"missed.jsonl",
				output,
				new StringWriter(),
				() => new Image { Width = 2, Height = 2 },
				(_, path) =>
				{
					File.WriteAllBytes(path, [1]);
					return 1;
				},
				playbackPath,
				atariPresentationEnabled: false);

			CapturePlaybackFrame(runtime, framebuffer, 0, 98);
			long sequence = 1;
			foreach (int boundary in Level7FramebufferDiagnostic.PlaybackCaptureBoundaries)
			{
				if (boundary == 100)
					continue;
				CapturePlaybackFrame(runtime, framebuffer, sequence++, boundary);
			}

			Assert.True(framebuffer.IsStopped);
			Assert.Equal(
				Level7FramebufferDiagnostic.MaximumPlaybackFrames - 1,
				framebuffer.CapturedFrameCount);
			Assert.Contains("100", output.ToString(), StringComparison.Ordinal);
			Assert.Contains("incomplete", output.ToString(), StringComparison.Ordinal);
			Assert.DoesNotContain(
				Directory.GetFiles(framebuffer.OutputDirectory, "*.png"),
				path => Path.GetFileName(path).Contains("00100", StringComparison.Ordinal));
		}
		finally
		{
			if (Directory.Exists(directory))
				Directory.Delete(directory, recursive: true);
		}
	}

	[Fact]
	public void CapturesThreeBoundedDepthBurstsWithSynchronizedTankerMetadata()
	{
		string directory = Path.Combine(
			Path.GetTempPath(), $"tempest-level7-framebuffer-{Guid.NewGuid():N}");
		string runtimePath = Path.Combine(directory, "runtime.jsonl");
		int readbackCount = 0;
		try
		{
			using Level7VisibleRuntimeDiagnostic runtime = new(
				runtimePath, "manualGameplay", null, "ExperimentalPortrait", 60,
				framebufferCaptureEnabled: true,
				atariPresentationEnabled: true);
			using Level7FramebufferDiagnostic framebuffer = CreateDiagnostic(
				directory,
				"manual.jsonl",
				new StringWriter(),
				new StringWriter(),
				() =>
				{
					readbackCount++;
					return new Image { Width = 2, Height = 2 };
				},
				(_, path) =>
				{
					File.WriteAllBytes(path, [1, 2, 3]);
					return 3;
				});
			object tanker = new();

			for (int sequence = 0; sequence < 172; sequence++)
			{
				float depth = .995f - sequence * .005f;
				CaptureFrame(runtime, framebuffer, tanker, sequence, depth);
			}
			runtime.Dispose();

			Assert.True(framebuffer.IsStopped);
			Assert.Equal(3, framebuffer.CompletedBurstCount);
			Assert.True(framebuffer.CapturedBytes <= Level7FramebufferDiagnostic.MaximumTotalBytes);
			Assert.Equal(
				Level7FramebufferDiagnostic.MaximumTotalFrames,
				framebuffer.CapturedFrameCount);
			Assert.Equal(Level7FramebufferDiagnostic.MaximumTotalFrames, readbackCount);
			Assert.Equal(
				Level7FramebufferDiagnostic.MaximumTotalFrames,
				Directory.GetFiles(framebuffer.OutputDirectory, "*.png").Length);

			string[] lines = File.ReadAllLines(runtimePath);
			using (JsonDocument run = JsonDocument.Parse(lines[0]))
				Assert.Contains(
					"selected actual screen-buffer pixels",
					run.RootElement.GetProperty("presentationTiming").GetString());
			JsonDocument[] captureFrames = lines
				.Where(line => line.Contains("\"framebufferCapture\"", StringComparison.Ordinal))
				.Select(line => JsonDocument.Parse(line))
				.ToArray();
			Assert.Equal(Level7FramebufferDiagnostic.MaximumTotalFrames, captureFrames.Length);
			Assert.Equal(
				3,
				captureFrames
					.Select(document => document.RootElement
						.GetProperty("framebufferCapture")
						.GetProperty("depthBand")
						.GetString())
					.Distinct(StringComparer.Ordinal)
					.Count());

			foreach (JsonDocument document in captureFrames)
			{
				JsonElement frame = document.RootElement;
				JsonElement capture = frame.GetProperty("framebufferCapture");
				JsonElement target = capture.GetProperty("tanker");
				long frameSequence = capture.GetProperty("renderFrameSequence").GetInt64();
				Assert.Equal(frameSequence, frame.GetProperty("renderFrameSequence").GetInt64());
				AssertEntityMatchesCapture(lines, frameSequence, target);
				Assert.Equal("Climbing", target.GetProperty("state").GetString());
				Assert.Equal(2, target.GetProperty("lane").GetInt32());
				Assert.True(target.GetProperty("fixedLaneContinuous").GetBoolean());
				Assert.Equal("PNG", capture.GetProperty("imageFormat").GetString());
				Assert.Equal(2, capture.GetProperty("imageWidth").GetInt32());
				Assert.Equal(2, capture.GetProperty("imageHeight").GetInt32());
				Assert.Equal(
					framebuffer.OutputDirectory,
					capture.GetProperty("outputDirectory").GetString());
				Assert.True(File.Exists(Path.Combine(
					framebuffer.OutputDirectory,
					capture.GetProperty("fileName").GetString()!)));
				Assert.True(capture.GetProperty("readbackDurationMilliseconds").GetDouble() >= 0);
				Assert.True(capture.GetProperty("pngWriteDurationMilliseconds").GetDouble() >= 0);
				Assert.True(frame.TryGetProperty("renderCallDurationSeconds", out _));
			}

			foreach (JsonDocument document in captureFrames)
				document.Dispose();
		}
		finally
		{
			if (Directory.Exists(directory))
				Directory.Delete(directory, recursive: true);
		}
	}

	[Fact]
	public void StopsAndRemovesAnImageThatExceedsThePerFrameDiskLimit()
	{
		string directory = Path.Combine(
			Path.GetTempPath(), $"tempest-level7-framebuffer-limit-{Guid.NewGuid():N}");
		string runtimePath = Path.Combine(directory, "runtime.jsonl");
		int readbackCount = 0;
		StringWriter output = new();
		StringWriter error = new();
		try
		{
			using Level7VisibleRuntimeDiagnostic runtime = new(
				runtimePath, "manualGameplay", null, "ExperimentalPortrait", 60,
				framebufferCaptureEnabled: true,
				atariPresentationEnabled: true);
			using Level7FramebufferDiagnostic framebuffer = CreateDiagnostic(
				directory,
				"limit.jsonl",
				output,
				error,
				() =>
				{
					readbackCount++;
					return new Image { Width = 2, Height = 2 };
				},
				(_, path) =>
				{
					File.WriteAllBytes(path, [1]);
					return Level7FramebufferDiagnostic.MaximumPngFileBytes + 1;
				});
			object tanker = new();

			CaptureFrame(runtime, framebuffer, tanker, 0, .99f);
			CaptureFrame(runtime, framebuffer, tanker, 1, .985f);
			CaptureFrame(runtime, framebuffer, tanker, 2, .98f);

			Assert.True(framebuffer.IsStopped);
			Assert.Equal(1, readbackCount);
			Assert.Equal(0, framebuffer.CapturedFrameCount);
			Assert.Empty(Directory.GetFiles(framebuffer.OutputDirectory, "*.png"));
			Assert.Contains("disk limit", error.ToString(), StringComparison.OrdinalIgnoreCase);
			Assert.Contains("No further framebuffer readback", output.ToString());
		}
		finally
		{
			if (Directory.Exists(directory))
				Directory.Delete(directory, recursive: true);
		}
	}

	[Fact]
	public void RejectsAnExistingCaptureFolderWithoutOverwritingIt()
	{
		string directory = Path.Combine(
			Path.GetTempPath(), $"tempest-level7-framebuffer-existing-{Guid.NewGuid():N}");
		string existingOutput = Path.Combine(directory, "framebuffer", "existing");
		Directory.CreateDirectory(existingOutput);
		try
		{
			Assert.Throws<IOException>(() => CreateDiagnostic(
				directory,
				"existing.jsonl",
				new StringWriter(),
				new StringWriter(),
				() => new Image { Width = 2, Height = 2 },
				(_, _) => 0));
			Assert.True(Directory.Exists(existingOutput));
		}
		finally
		{
			if (Directory.Exists(directory))
				Directory.Delete(directory, recursive: true);
		}
	}

	private static Level7FramebufferDiagnostic CreateDiagnostic(
		string directory,
		string fileName,
		TextWriter output,
		TextWriter error,
		Func<Image> readFramebuffer,
		Func<Image, string, long> writePng,
		string? playbackFilePath = null,
		bool atariPresentationEnabled = true) =>
		new(
			Path.Combine(directory, "framebuffer"),
			fileName,
			output,
			error,
			readFramebuffer,
			writePng,
			_ => { },
			playbackFilePath,
			atariPresentationEnabled);

	private static void CapturePlaybackFrame(
		Level7VisibleRuntimeDiagnostic runtime,
		Level7FramebufferDiagnostic framebuffer,
		long renderFrameSequence,
		int playbackBoundary)
	{
		long renderStart = renderFrameSequence * 1_000L + 100;
		runtime.BeginFrame(
			renderFrameSequence,
			playbackBoundary,
			playbackBoundary,
			7,
			LevelState.Playing,
			false,
			DisplayConfiguration.ExperimentalPortraitPlayfieldCamera,
			renderStart,
			"recordedPlayback");
		runtime.CaptureFramebufferFrame(framebuffer);
		runtime.EndFrame(renderStart + 10);
	}

	private static void CaptureFrame(
		Level7VisibleRuntimeDiagnostic runtime,
		Level7FramebufferDiagnostic framebuffer,
		object tanker,
		int sequence,
		float normalizedDepth)
	{
		long renderStart = sequence * 1_000L + 100;
		runtime.BeginFrame(
			sequence,
			sequence + 1,
			null,
			7,
			LevelState.Playing,
			false,
			DisplayConfiguration.ExperimentalPortraitPlayfieldCamera,
			renderStart);
		runtime.CaptureEntity(
			tanker,
			"tanker",
			"Climbing",
			2,
			10 + sequence * 1.75f,
			10,
			new(100, 200 - sequence),
			normalizedDepth,
			new(100, 200 - sequence),
			.2f,
			6.2f,
			[new(99, 199 - sequence), new(101, 201 - sequence)],
			1);
		runtime.CaptureFramebufferFrame(framebuffer);
		runtime.EndFrame(renderStart + 10);
	}

	private static void AssertEntityMatchesCapture(
		string[] lines,
		long expectedSequence,
		JsonElement target)
	{
		foreach (string line in lines)
		{
			using JsonDocument entity = JsonDocument.Parse(line);
			JsonElement record = entity.RootElement;
			if (record.GetProperty("recordType").GetString() != "entity" ||
				record.GetProperty("frameSequence").GetInt64() != expectedSequence ||
				record.GetProperty("entityId").GetInt64() != target.GetProperty("entityId").GetInt64())
			{
				continue;
			}

			Assert.Equal("Climbing", record.GetProperty("state").GetString());
			Assert.True(record.GetProperty("fixedLaneContinuous").GetBoolean());
			Assert.Equal(target.GetProperty("lane").GetInt32(), record.GetProperty("lane").GetInt32());
			Assert.Equal(target.GetProperty("height").GetSingle(), record.GetProperty("height").GetSingle());
			Assert.Equal(
				target.GetProperty("normalizedDepth").GetSingle(),
				record.GetProperty("normalizedDepth").GetSingle());
			Assert.True(record.TryGetProperty("mappedAnchor", out _));
			Assert.True(record.TryGetProperty("hostAnchor", out _));
			Assert.True(record.TryGetProperty("hostGeometryBounds", out _));
			Assert.True(record.TryGetProperty("logicalGeometryBounds", out _));
			return;
		}

		throw new Xunit.Sdk.XunitException(
			$"No Tanker entity record was found for render frame {expectedSequence}.");
	}
}
