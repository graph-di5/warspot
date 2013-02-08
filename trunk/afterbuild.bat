echo off
echo this script makes redist package
rem echo on

xcopy /s /y bin\Content redistr\Content\
copy /y bin\nuclex.*.dll redistr\
copy /y bin\nunit.framework.dll redistr\
copy /y bin\SlimDX.DirectInput.dll redistr\
copy /y bin\WarSpot.Client.XnaClient.exe redistr\
copy /y bin\WarSpot.Common.dll redistr\
copy /y bin\WarSpot.MatchComputer.dll redistr\
copy /y bin\WarSpot.ConsoleComputer.exe redistr\
copy /y bin\WarSpot.Contracts.Intellect.dll redistr\
copy /y bin\WarSpot.Contracts.Service.dll redistr\
 
rem      