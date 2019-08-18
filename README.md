# BattleShipGame
A single-player game of BattleShip. The computer generates randomly 3 ships (2 ships of size 4 cells
, 1 ship of size 5 cells). The users inserts coordinates where he wants to hit. The coordinates use the following format: "{char}{number}", where "char" is from A->J and number is from 0->9. The game is won when the user destroyed all the ships. After each hit he is presented with a score that represents the number of bullets he fired. The lower the score the better. 

## Requirements
- .NET Core >= 2.2
- Linux/macOS/Windows

## Building
```shell
git clone https://github.com/KangJiYoung/BattleShipGame.git
cd BattleShip.Console
dotnet restore
dotnet run
```