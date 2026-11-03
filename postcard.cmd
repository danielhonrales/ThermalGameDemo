@echo off
rem Flashpoint postcard booth: winner and loser photos -> shareable postcard (saved in tools\postcard\output).
rem   postcard.cmd              use the built-in webcam
rem   postcard.cmd --camera 2   use another webcam by index (2 = OBSBOT)
python "%~dp0tools\postcard\postcard_app.py" %*
if errorlevel 1 pause
