﻿#summary One-sentence summary of this page.
#labels Phase-Deploy

## Introduction ##

Некоторые заметки о деплое


## Деплой базы ##

деплоить базу в ажур надо редко, перед заливкой новый схемы из автогенеренного **обязательно** изменить все индексы на `CLUSTERED`
<br />
есть известная проблема в entity framework + sql azure http://entityframework.codeplex.com/workitem/592 , описанная процедура нужна для обхода ошибки, пока не выйдет исправление проблемы в EF6


надо подумать, как деплоить базу без затирания данных:
  * дампы
  * ручками править скрипт новой схемы
  * сразу писать обновления схемы как пачки скриптов