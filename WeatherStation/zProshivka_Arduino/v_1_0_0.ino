#include <TinyDHT.h>

// Определяем пин, к которому подключен датчик DHT11
#define DHTPIN A0
// Определяем тип подключенного датчика
#define DHTTYPE DHT11

// Создаем экземпляр датчика
DHT dht(DHTPIN, DHTTYPE);

void setup() {
  // Настраиваем последовательное соединение со скоростью 9600 бод
  Serial.begin(9600);
  // Инициализируем датчик DHT11
  dht.begin();
}

void loop() {
  
  // Считываем значения температуры и влажности
  int H = dht.readHumidity();
  int T = dht.readTemperature();
  
  // Проверяем, не произошла ли ошибка при чтении данных
  if (isnan(H) || isnan(T)) {
    Serial.println("Failed to read from DHT sensor!");
  } else {
    // Выводим значения температуры и влажности в последовательный порт в формате "t:h"
    Serial.print(T);
    Serial.print(":");
    Serial.println(H);
  }
  
  // Задержка в 2 секунды перед следующим измерением
  delay(2000);
}
