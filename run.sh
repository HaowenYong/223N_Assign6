rm *.dll
rm *.exe

ls -l

mcs -target:library -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:BallTracing.dll BallTracing.cs

mcs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:BallTracing.dll -out:ball.exe BallTracingMain.cs

./ball.exe