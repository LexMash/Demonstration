Репозиторий содержит фичи/сервисы сделанные для одного инди проекта.
Проект использовал старую версию архитектуры Lukomor.
Новые версии доступны по ссылке - https://github.com/vavilichev/Lukomor

Особенность архитектуры в том, что каждая глобальная фича/сервис использует набор интеракторов в виде интерфейса.
А так же, SignalTower для отправки сигналов (Signal) всем подписчикам, что является аналогом решения от Zenject и реализацией паттерна Observer.

Здесь представленны следующие фичи:
1. система контенеров ContainerSystem
2. система совмещения и разделения предметов (крафта) CraftSystem
3. инвентарь в UI представлении
4. система динамически наполняемого дневника для главной героини - NotepadSystem

