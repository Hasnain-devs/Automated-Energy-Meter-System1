@echo off
set BASE=C:\Users\Hp\mariadb\mariadb-12.2.2-winx64
set CFG=C:\Users\Hp\mariadb\data\my.ini
netstat -ano | findstr /R /C:":3306 .*LISTENING" >nul
if %errorlevel%==0 (
  echo MariaDB already running on port 3306.
  goto :end
)

powershell -NoProfile -ExecutionPolicy Bypass -Command "Start-Process -FilePath '%BASE%\bin\mysqld.exe' -ArgumentList '--defaults-file=%CFG%' -WindowStyle Hidden"
powershell -NoProfile -ExecutionPolicy Bypass -Command "Start-Sleep -Seconds 5"

netstat -ano | findstr /R /C:":3306 .*LISTENING" >nul
if %errorlevel%==0 (
  echo MariaDB started on port 3306.
) else (
  echo MariaDB did not start. Check C:\Users\Hp\mariadb\data\DESKTOP-2715FOH.err
)

:end
