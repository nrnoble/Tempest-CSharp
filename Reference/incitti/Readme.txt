Tempest 3.1
---------------------------------------------------------------------------
Get latest version here:  http:\\www.incitti.com\Blitz\


History:
---------------------------------------------------------------------------

' Version 3.1 - April 17, 2008
' added some mouse control to the editor & game
' couple more level sets


Version 3.0 - April 10
  back to (16 boards, 6 colours)
  level sets  5!
  integrated the level editor
  added a new level set
  renamed T3MP3ST
  rotation after level 55

Version 2.1 - April 7
 adjust spike growth and shot damage
 fix level # on game over screen
 add hi score to game over
 added rotation levels

Version 2.0 - April 3, 2008
   various bug fixes 
   better difficulty scaling
   updated title screen
   added icon, min/max buttons, app terminate, etc
   made all levels available at select screen


Version 1.5 - July 18, 2005
   added 16 more tubes
   tweaked the difficulty and enemy release rates
   changed many of the scaling calcs to TFormSZ()

Version 1.1 - July 7, 2005
   fixed egg that were hatching to act 7 (non-existent case 
   - they disappeared)
   changed fuseballs - only kill when off the edge  w>1 or w<7
   added Tempest Tubes boards

Version 1.0 - June 20, 2005
   Completed by Mark Incitti (Mark1nc)

Version 0.1 - 
   Started by David Bird (Birdie) - BlitzMax 1.10 Samples 




Instructions:
------------------------------------------------------------------------------
Run Tempest.exe
Shoot everything.


Controls:
------------------------------------------------------------------------------
CTRL - Fire!
SPACE - Superzapper
L/R - Move
ESC - Quit
T - select tube set  (3 sets of 16 tubes)



Building using BMax:
------------------------------------------------------------------------------
Run boardgen.bmx first - it creates the file boarddata.bmx 
which contains the tube co-ords.
This file will be included into Tempest.bmx
Compile Tempest.bmx
Feel free to modify and tweak - please let me know of any
cool changes.

icon:
windres -i D:\Data\From-old-laptop\Personal\Programming\Blitz\max\tempest\resfile.rc -o D:\Data\From-old-laptop\Personal\Programming\Blitz\max\tempest\projecticon.o


