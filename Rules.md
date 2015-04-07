#labels Featured,Phase-Implementation
Для описания регламента турниров см. [Tournaments](Tournaments.md).

Матч WarSpot проходит в мире-сетке (см. [WorldDescription](WorldDescription.md)), в котором первоначально в случайных позициях создаётся по одному существу для каждого участника, а также клетки мира заполняются энергией. Существо под контролем написанного игроком интеллекта может переходить из клетки в клетку, атаковать других существ и поглощать энергию (подробнее о существе и использовании энергии см. [GameActionsFullDescription](GameActionsFullDescription.md)) Цель игры - уничтожить все враждебные существа.

Игрок должен предоставить .NET библиотеку, содержащую класс существа. Класс существа обязательно должен реализовывать интерфейс [IBeingInterface](http://code.google.com/p/warspot/source/browse/trunk/WarSpot.Contracts.Intellect/IBeingInterface.cs). Первый метод Construct вызывается для создания существа, именно в нем задаются желаемые характеристики создаваемого экземпляра. Второй метод Think вызывается один раз за ход для каждого экземпляра. Параметрами является иформация о мире, возвращаемое значение должно быть действием. Действие - один из наследников абстрактного класса GameAction. На данный момент доступны следующие действия:
  * GameActionAttack
  * GameActionEat
  * GameActionGiveCi
  * GameActionMove
  * GameActionTreat
  * GameActionMakeOffspring

Определения всех необходимых типов находятся в библиотеке WarSpot.Contracts.Intellect, ее надо необходимо включать в References проекта интеллекта.

Код библиотеки WarSpot.Contracts.Intellect и тестового интеллекта снабжены подробными комментариями, которые помогут разобраться в деталях.

[WorldDescription](WorldDescription.md)
[GameActionsFullDescription](GameActionsFullDescription.md)
[Tournaments](Tournaments.md)