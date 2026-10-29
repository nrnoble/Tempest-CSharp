using System.Numerics;
using Raylib_cs;
using Tempest.Game.Rendering;

const int screenWidth = 800;
const int screenHeight = 600;

Vector2[] level1 =
[
    new( 200,    0), new( 184,   76), new( 141,  141), new(  76,  184),
    new(   0,  200), new( -76,  184), new(-141,  141), new(-184,   76),
    new(-200,    0), new(-184,  -76), new(-141, -141), new( -76, -184),
    new(   0, -200), new(  76, -184), new( 141, -141), new( 184,  -76)
];

const float centerY = 400;
const float yOffset = -80;
const float tubeDepth = 400;
const float tubePosition = 50;

const float playerSpeed = 0.05f;
const float spinnerSensitivity = 0.0125f;
const int maximumShots = 10;
const float shotSpeed = 6;

const float flipperClimbSpeed = 2;
const float flipperTurnSpeedDegrees = 8;
const int flipperRimPauseTicks = 24;
const int playerDeathTicks = 60;

// Level 1 test wave. Incitti's Level 1 setup begins with 10 eggs/enemies.
const int levelEnemyTotal = 10;
const int spawnIntervalTicks = 50;
const int maximumActiveFlippers = 4;

int playerEdge = 7;
float playerPosition = 0.5f;
int lives = 3;
int deathTicks = 0;
bool gameOver = false;
bool levelComplete = false;

int shotTimer = 3;
int score = 0;
int enemiesSpawned = 0;
int enemiesDestroyed = 0;
int spawnTimer = 0;
int nextSpawnLaneIndex = 0;

// Deterministic lanes make this milestone repeatable while testing.
int[] spawnLanes = [12, 4, 9, 1, 14, 6, 10, 2, 15, 7];

List<PlayerShot> playerShots = [];
List<FlipperTarget> flippers = [];

Projection.K = 70;
Projection.ZOffset = 5;

Raylib.InitWindow(screenWidth, screenHeight, "Tempest C# - Level 1 Multi-Flipper Test");
Raylib.DisableCursor();
Raylib.SetTargetFPS(60);

ResetLevel();

while (!Raylib.WindowShouldClose())
{
    if (gameOver)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.R))
            ResetGame();
    }
    else if (levelComplete)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.R))
            ResetLevel();
    }
    else if (deathTicks > 0)
    {
        UpdatePlayerDeath();
    }
    else
    {
        UpdatePlayer();
        UpdatePlayerShots();
        UpdateEnemySpawning();
        UpdateFlippers();
        CheckFlippersAgainstPlayer();
        CheckForLevelComplete();
    }

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);

    DrawTube(level1);
    DrawPlayerShots(level1);
    DrawFlippers(level1);

    if (!gameOver)
    {
        if (deathTicks > 0)
            DrawPlayerExplosion(level1);
        else
            DrawPlayer(level1);
    }

    DrawHud();
    Raylib.EndDrawing();
}

Raylib.EnableCursor();
Raylib.CloseWindow();

void ResetGame()
{
    score = 0;
    lives = 3;
    gameOver = false;
    ResetLevel();
}

void ResetLevel()
{
    levelComplete = false;
    deathTicks = 0;
    enemiesSpawned = 0;
    enemiesDestroyed = 0;
    spawnTimer = 0;
    nextSpawnLaneIndex = 0;
    ResetPlayerPosition();
    playerShots.Clear();
    flippers.Clear();
}

void ResetAfterDeath()
{
    ResetPlayerPosition();
    playerShots.Clear();
    flippers.Clear();
    spawnTimer = 0;

    // Respawn only the enemies not yet destroyed. Previously spawned but
    // surviving test Flippers return to the pending pool after a death.
    enemiesSpawned = enemiesDestroyed;
    nextSpawnLaneIndex = enemiesDestroyed % spawnLanes.Length;
}

void ResetPlayerPosition()
{
    playerEdge = 7;
    playerPosition = 0.5f;
    shotTimer = 3;
}

void UpdatePlayerDeath()
{
    deathTicks--;
    if (deathTicks > 0)
        return;

    lives--;
    if (lives <= 0)
    {
        gameOver = true;
        playerShots.Clear();
        flippers.Clear();
        return;
    }

    ResetAfterDeath();
}

void KillPlayer()
{
    if (deathTicks > 0 || gameOver)
        return;

    deathTicks = playerDeathTicks;
    playerShots.Clear();
}

void UpdatePlayer()
{
    UpdatePlayerFiring();

    Vector2 mouseDelta = Raylib.GetMouseDelta();
    float movement = mouseDelta.X * spinnerSensitivity;

    if (Raylib.IsKeyDown(KeyboardKey.Left))
        movement -= playerSpeed;
    if (Raylib.IsKeyDown(KeyboardKey.Right))
        movement += playerSpeed;

    playerPosition += movement;

    while (playerPosition < 0)
    {
        playerEdge = PreviousEdge(playerEdge, level1.Length);
        playerPosition += 1;
    }

    while (playerPosition > 1)
    {
        playerEdge = NextEdge(playerEdge, level1.Length);
        playerPosition -= 1;
    }
}

void UpdatePlayerFiring()
{
    bool fireButtonDown =
        Raylib.IsKeyDown(KeyboardKey.LeftControl) ||
        Raylib.IsKeyDown(KeyboardKey.RightControl) ||
        Raylib.IsMouseButtonDown(MouseButton.Left);

    if (fireButtonDown && shotTimer > 2 && playerShots.Count < maximumShots)
    {
        playerShots.Add(new PlayerShot(playerEdge, tubeDepth));
        shotTimer = 0;
    }

    shotTimer++;
}

void UpdatePlayerShots()
{
    for (int shotIndex = playerShots.Count - 1; shotIndex >= 0; shotIndex--)
    {
        PlayerShot shot = playerShots[shotIndex];
        shot.Depth -= shotSpeed;
        shot.Rotation = (shot.Rotation + 15) % 360;

        if (CheckShotAgainstFlippers(shot) || shot.Depth < 0)
            playerShots.RemoveAt(shotIndex);
    }
}

bool CheckShotAgainstFlippers(PlayerShot shot)
{
    for (int i = flippers.Count - 1; i >= 0; i--)
    {
        FlipperTarget flipper = flippers[i];
        bool matchingEdge = shot.Edge == flipper.Edge;

        if (flipper.State == FlipperState.Flipping)
            matchingEdge = shot.Edge == flipper.SourceEdge ||
                           shot.Edge == flipper.DestinationEdge;

        if (!matchingEdge)
            continue;

        float separation = flipper.Height - shot.Depth;
        if (separation <= 0 || separation >= 16)
            continue;

        flippers.RemoveAt(i);
        enemiesDestroyed++;
        score += 150;
        return true;
    }

    return false;
}

void UpdateEnemySpawning()
{
    if (enemiesSpawned >= levelEnemyTotal ||
        flippers.Count >= maximumActiveFlippers)
        return;

    spawnTimer++;
    if (spawnTimer < spawnIntervalTicks)
        return;

    int lane = spawnLanes[nextSpawnLaneIndex % spawnLanes.Length];
    nextSpawnLaneIndex++;
    flippers.Add(new FlipperTarget(lane, 0));
    enemiesSpawned++;
    spawnTimer = 0;
}

void UpdateFlippers()
{
    foreach (FlipperTarget flipper in flippers)
    {
        switch (flipper.State)
        {
            case FlipperState.Climbing:
                flipper.Height += flipperClimbSpeed;
                if (flipper.Height >= tubeDepth)
                {
                    flipper.Height = tubeDepth;
                    BeginFlipperPause(flipper);
                }
                break;

            case FlipperState.Pausing:
                flipper.PauseTicks--;
                if (flipper.PauseTicks <= 0)
                    BeginFlipperPivot(flipper);
                break;

            case FlipperState.Flipping:
                flipper.TurnAngle += flipperTurnSpeedDegrees;
                if (flipper.TurnAngle >= flipper.TotalTurnAngle)
                    CompleteFlipperPivot(flipper);
                break;
        }
    }
}

void CheckFlippersAgainstPlayer()
{
    // A completed pivot marks collision. The whole visible pivot remains
    // a shooting opportunity; CompleteFlipperPivot sets this flag only
    // after the Flipper fully occupies the Blaster's lane.
    foreach (FlipperTarget flipper in flippers)
    {
        if (!flipper.CompletedIntoPlayer)
            continue;

        flipper.CompletedIntoPlayer = false;
        KillPlayer();
        return;
    }
}

void BeginFlipperPause(FlipperTarget flipper)
{
    flipper.State = FlipperState.Pausing;
    flipper.PauseTicks = flipperRimPauseTicks;
    flipper.TurnAngle = 0;
}

void BeginFlipperPivot(FlipperTarget flipper)
{
    flipper.Direction = FindShortestDirection(flipper.Edge, playerEdge, level1.Length);
    flipper.SourceEdge = flipper.Edge;
    flipper.DestinationEdge = flipper.Direction > 0
        ? PreviousEdge(flipper.Edge, level1.Length)
        : NextEdge(flipper.Edge, level1.Length);
    flipper.TotalTurnAngle = CalculateEdgeTurnAngle(
        flipper.SourceEdge, flipper.DestinationEdge);
    flipper.TurnAngle = 0;
    flipper.State = FlipperState.Flipping;
}

void CompleteFlipperPivot(FlipperTarget flipper)
{
    flipper.Edge = flipper.DestinationEdge;
    flipper.SourceEdge = flipper.Edge;
    flipper.TurnAngle = 0;
    flipper.TotalTurnAngle = 0;

    if (flipper.Edge == playerEdge)
    {
        flipper.CompletedIntoPlayer = true;
        return;
    }

    BeginFlipperPause(flipper);
}

void CheckForLevelComplete()
{
    if (enemiesDestroyed == levelEnemyTotal && flippers.Count == 0)
        levelComplete = true;
}

int FindShortestDirection(int fromEdge, int toEdge, int edgeCount)
{
    int clockwise = (toEdge - fromEdge + edgeCount) % edgeCount;
    int counterclockwise = (fromEdge - toEdge + edgeCount) % edgeCount;
    return clockwise < counterclockwise ? -1 : 1;
}

int NextEdge(int edge, int edgeCount) => (edge + 1) % edgeCount;
int PreviousEdge(int edge, int edgeCount) => (edge - 1 + edgeCount) % edgeCount;

float CalculateEdgeTurnAngle(int sourceEdge, int destinationEdge)
{
    int sourceNext = NextEdge(sourceEdge, level1.Length);
    int destinationNext = NextEdge(destinationEdge, level1.Length);
    Vector2 sourceVector = level1[sourceNext] - level1[sourceEdge];
    Vector2 destinationVector = level1[destinationNext] - level1[destinationEdge];
    float sourceAngle = MathF.Atan2(sourceVector.Y, sourceVector.X);
    float destinationAngle = MathF.Atan2(destinationVector.Y, destinationVector.X);
    float difference = MathF.Abs(NormalizeAngleDegrees(
        (destinationAngle - sourceAngle) * 180 / MathF.PI));
    return difference > 180 ? 360 - difference : difference;
}

float NormalizeAngleDegrees(float angle)
{
    while (angle < 0) angle += 360;
    while (angle >= 360) angle -= 360;
    return angle;
}

void DrawTube(Vector2[] points)
{
    Vector2[] nearRim = new Vector2[points.Length];
    Vector2[] farRim = new Vector2[points.Length];

    for (int i = 0; i < points.Length; i++)
    {
        nearRim[i] = Projection.Project(
            points[i].X, points[i].Y, tubePosition, centerY, yOffset);
        farRim[i] = Projection.Project(
            points[i].X, points[i].Y, tubePosition + tubeDepth, centerY, yOffset);
    }

    for (int i = 0; i < points.Length; i++)
        Raylib.DrawLineV(nearRim[i], farRim[i], Color.Blue);

    for (int i = 0; i < points.Length; i++)
    {
        int next = NextEdge(i, points.Length);
        Raylib.DrawLineV(nearRim[i], nearRim[next], Color.Blue);
        Raylib.DrawLineV(farRim[i], farRim[next], Color.Blue);
    }
}

void DrawPlayerShots(Vector2[] points)
{
    foreach (PlayerShot shot in playerShots)
    {
        int next = NextEdge(shot.Edge, points.Length);
        Vector2 midpoint = new(
            (points[shot.Edge].X + points[next].X) / 2,
            (points[shot.Edge].Y + points[next].Y) / 2);
        float projectedDepth = tubeDepth - shot.Depth + tubePosition;
        Vector2 center = Projection.Project(
            midpoint.X, midpoint.Y, projectedDepth, centerY, yOffset);
        float size = 4 / ((projectedDepth + Projection.ZOffset) / Projection.K);
        DrawRotatingShot(center, size, shot.Rotation);
    }
}

void DrawRotatingShot(Vector2 center, float size, float rotation)
{
    for (int angle = 0; angle < 360; angle += 45)
    {
        float radians = (rotation + angle) * MathF.PI / 180;
        Vector2 offset = new(
            MathF.Cos(radians) * size,
            MathF.Sin(radians) * size);
        Raylib.DrawLineV(center - offset, center + offset, Color.White);
    }
}

void DrawFlippers(Vector2[] points)
{
    foreach (FlipperTarget flipper in flippers)
        DrawFlipper(points, flipper);
}

void DrawFlipper(Vector2[] points, FlipperTarget flipper)
{
    int edge = flipper.State == FlipperState.Flipping
        ? flipper.SourceEdge
        : flipper.Edge;
    int next = NextEdge(edge, points.Length);
    float projectedDepth = tubePosition + tubeDepth - flipper.Height;

    Vector2 p1 = Projection.Project(
        points[edge].X, points[edge].Y, projectedDepth, centerY, yOffset);
    Vector2 p2 = Projection.Project(
        points[next].X, points[next].Y, projectedDepth, centerY, yOffset);

    if (flipper.State == FlipperState.Flipping)
        ApplyFlipperPivot(flipper, ref p1, ref p2);

    DrawFlipperShape(p1, p2, projectedDepth);
}

void ApplyFlipperPivot(FlipperTarget flipper, ref Vector2 p1, ref Vector2 p2)
{
    float angle = MathF.Min(flipper.TurnAngle, flipper.TotalTurnAngle);
    if (flipper.Direction > 0)
        p2 = RotatePoint(p2, p1, -angle);
    else
        p1 = RotatePoint(p1, p2, angle);
}

Vector2 RotatePoint(Vector2 point, Vector2 pivot, float degrees)
{
    float radians = degrees * MathF.PI / 180;
    float sin = MathF.Sin(radians);
    float cos = MathF.Cos(radians);
    Vector2 translated = point - pivot;
    return pivot + new Vector2(
        translated.X * cos - translated.Y * sin,
        translated.X * sin + translated.Y * cos);
}

void DrawFlipperShape(Vector2 p1, Vector2 p2, float projectedDepth)
{
    float dx = p2.X - p1.X;
    float dy = p2.Y - p1.Y;
    float length = MathF.Sqrt(dx * dx + dy * dy);
    if (length == 0) return;

    float xn = -dy / length;
    float yn = dx / length;
    float height = 20 / ((projectedDepth + Projection.ZOffset) / Projection.K);

    Vector2 point2 = new(p2.X - dx * .3f - xn * height / 2,
                         p2.Y - dy * .3f - yn * height / 2);
    Vector2 point3 = new(p2.X - dx * .1f - xn * height,
                         p2.Y - dy * .1f - yn * height);
    Vector2 point5 = new(p2.X - dx * .9f - xn * height,
                         p2.Y - dy * .9f - yn * height);
    Vector2 point6 = new(p2.X - dx * .7f - xn * height / 2,
                         p2.Y - dy * .7f - yn * height / 2);

    Raylib.DrawLineV(p1, point3, Color.Red);
    Raylib.DrawLineV(point5, p2, Color.Red);
    Raylib.DrawLineV(point6, point5, Color.Red);
    Raylib.DrawLineV(point2, point3, Color.Red);
    Raylib.DrawLineV(point6, p1, Color.Red);
    Raylib.DrawLineV(point2, p2, Color.Red);
}

void DrawPlayer(Vector2[] points)
{
    GetPlayerEdge(points, out Vector2 p1, out Vector2 p2);
    float dx = p2.X - p1.X;
    float dy = p2.Y - p1.Y;
    float length = MathF.Sqrt(dx * dx + dy * dy);
    if (length == 0) return;

    float xn = -dy / length;
    float yn = dx / length;
    const float clawHeight = 20;
    Vector2 edgeVector = new(dx, dy);
    Vector2 center = p2 - edgeVector * playerPosition;
    Vector2 outer1 = center + new Vector2(xn, yn) * clawHeight;
    Vector2 outer2 = center + new Vector2(xn, yn) * clawHeight * .5f;
    Vector2 left1 = p2 - edgeVector * .2f;
    Vector2 left2 = p2 - edgeVector * .3f;
    Vector2 right1 = p2 - edgeVector * .7f;
    Vector2 right2 = p2 - edgeVector * .8f;

    Raylib.DrawLineV(left1, outer1, Color.Yellow);
    Raylib.DrawLineV(right1, outer1, Color.Yellow);
    Raylib.DrawLineV(left1, left2, Color.Yellow);
    Raylib.DrawLineV(right1, right2, Color.Yellow);
    Raylib.DrawLineV(outer2, left2, Color.Yellow);
    Raylib.DrawLineV(outer2, right2, Color.Yellow);
}

void GetPlayerEdge(Vector2[] points, out Vector2 p1, out Vector2 p2)
{
    int next = NextEdge(playerEdge, points.Length);
    p1 = Projection.Project(
        points[playerEdge].X, points[playerEdge].Y,
        tubePosition, centerY, yOffset);
    p2 = Projection.Project(
        points[next].X, points[next].Y,
        tubePosition, centerY, yOffset);
}

void DrawPlayerExplosion(Vector2[] points)
{
    GetPlayerEdge(points, out Vector2 p1, out Vector2 p2);
    Vector2 center = Vector2.Lerp(p2, p1, playerPosition);
    float progress = 1f - deathTicks / (float)playerDeathTicks;
    float radius = 8 + progress * 48;

    for (int angle = 0; angle < 360; angle += 45)
    {
        float radians = angle * MathF.PI / 180;
        Vector2 offset = new(
            MathF.Cos(radians) * radius,
            MathF.Sin(radians) * radius);
        Raylib.DrawLineV(center - offset * .25f, center + offset, Color.Yellow);
    }
}

void DrawHud()
{
    Raylib.DrawText(score.ToString(), 10, 10, 28, Color.Yellow);
    Raylib.DrawText($"LIVES {lives}", 10, 44, 20, Color.Yellow);
    int remaining = levelEnemyTotal - enemiesDestroyed;
    Raylib.DrawText($"FLIPPERS {remaining}", 10, 72, 18, Color.Red);
    Raylib.DrawText($"ACTIVE {flippers.Count}", 10, 96, 18, Color.Gray);

    if (gameOver)
    {
        DrawCenteredText("GAME OVER - PRESS R", 270, 32, Color.Red);
        return;
    }

    if (levelComplete)
    {
        DrawCenteredText("LEVEL 1 COMPLETE", 250, 36, Color.Yellow);
        DrawCenteredText("PRESS R TO REPLAY", 296, 24, Color.Gray);
        return;
    }

    if (deathTicks > 0)
        Raylib.DrawText("PLAYER DESTROYED", 10, 120, 18, Color.Red);
}

void DrawCenteredText(string text, int y, int size, Color color)
{
    int width = Raylib.MeasureText(text, size);
    Raylib.DrawText(text, (screenWidth - width) / 2, y, size, color);
}

sealed class PlayerShot
{
    public PlayerShot(int edge, float depth)
    {
        Edge = edge;
        Depth = depth;
    }

    public int Edge { get; }
    public float Depth { get; set; }
    public float Rotation { get; set; }
}

enum FlipperState
{
    Climbing,
    Pausing,
    Flipping
}

sealed class FlipperTarget
{
    public FlipperTarget(int edge, float height)
    {
        Edge = edge;
        SourceEdge = edge;
        DestinationEdge = edge;
        Height = height;
        State = FlipperState.Climbing;
    }

    public int Edge { get; set; }
    public int SourceEdge { get; set; }
    public int DestinationEdge { get; set; }
    public float Height { get; set; }
    public FlipperState State { get; set; }
    public int PauseTicks { get; set; }
    public int Direction { get; set; }
    public float TurnAngle { get; set; }
    public float TotalTurnAngle { get; set; }
    public bool CompletedIntoPlayer { get; set; }
}
