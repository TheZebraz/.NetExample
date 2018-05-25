# .NetExample
Данный репозиторий включает реализацию API сервиса по разгадыванию капч [cptch.net](https://cptch.net) на C#.
Пример представляет собой консольное приложение. 

Кроме того, проект содержит реализацию интерфейса ICaptchaSolver из популярной реализации API Вконтакте для .Net.

# Как использовать с Vk Api for .NET

Чтобы начать использовать реализацию интерфейса ICaptchaSolver библиотеки [Vk Api for .NET](https://github.com/vknet/vk), нужно:
1. Скопируйте класс [CptchCaptchaSolver](https://github.com/cptch/.NetExample/blob/master/CptchCaptchaSolving/CptchCaptchaSolver.cs) в свой проект.
2. Переименуйте namespace CptchCaptchaSolving на пространство имен вашего приложения.
3. В классе CptchCaptchaSolving измените значение переменной CPTCH_API_KEY на ваш Api ключ, взяв его [здесь](http://cptch.net/profile)
4. Подключите библиотеку [Vk Api for .NET](https://github.com/vknet/vk).
5. В конструктор объекта VkApi передайти объект класса CptchCaptchaSolver
<br>Это можно сделать, например, так:<br>
```VkApi vkApi = new VkApi(new CptchCaptchaSolving());```
6. Готово. Теперь капчи будут решаться через [cptch.net](https://cptch.net/) — самый дешевый сервис по разгадыванию капч:)

Если у вас что-то не получилось или вы нашли ошибку, смело пишите нам в поддержку support@cptch.net. Будем рады помочь вам!
