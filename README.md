# BattleShipGame
A single-player game of BattleShip. The computer generates randomly 3 ships (2 ships of size 4 cells
, 1 ship of size 5 cells). The users inserts coordinates where he wants to hit. The coordinates use the following format: "{char}{number}", where "char" is from A->J and number is from 0->9. The game is won when the user destroyed all the ships. After each hit he is presented with a score that represents the number of bullets he fired. The lower the score the better. 

## Requirements
- .NET Core >= 2.2
- Linux/macOS/Windows

## Building
Clone the repository
```shell
git clone https://github.com/KangJiYoung/BattleShipGame.git
```
Change directory to main folder
```shell
cd BattleShip.Console
```
Restore the packages
```shell
dotnet restore
```
Run the application
```shell
dotnet run
```

## Releases
You can also use the existing releases
- For windows: https://github.com/KangJiYoung/BattleShipGame/releases/tag/1.0.0%40win-x64
  - Download the ZIP Folder
  - Unzip it
  - Go to publish folder
  - Execute BattleShip.Console.exe
- For osx: https://github.com/KangJiYoung/BattleShipGame/releases/tag/1.0.0%40osx
  - Download the ZIP Folder
  - Unzip it
  - Go to publish folder using a terminal
  - Execute the command
  ```
   ./BattleShip.Console
  ```
