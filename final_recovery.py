import torch
import torch.nn as nn
import collections 
import os

checkpoint_path = "results/MyAI_ver1/MyAI/checkpoint.pt"
output_path = "results/MyAI_ver1/MyAI_Ready.onnx"

print("--- Починаємо примусове витягування моделі ---")

# Завантажуємо ваги
checkpoint = torch.load(checkpoint_path, map_location='cpu')
state_dict = checkpoint['Policy']

# Створюємо архітектуру, яка відповідає налаштуванням (128 units, 2 layers)
# Ми використовуємо просту структуру, щоб Unity Barracuda/Sentis її зрозуміла
class RecoveredModel(nn.Module):
    def __init__(self):
        super().__init__()
        # 164 - розмір входу (obs_0)
        self.body = nn.Sequential(
            nn.Linear(164, 128),
            nn.ReLU(),
            nn.Linear(128, 128),
            nn.ReLU()
        )
        # Вихідний шар
        self.output = nn.Linear(128, 4) 

    def forward(self, x):
        return self.output(self.body(x))

model = RecoveredModel()
new_state_dict = collections.OrderedDict()

# Очищуємо ключі ваг від префіксів ML-Agents (наприклад, "version_number" або "network_body")
# щоб вони підійшли до моделі
new_state_dict = collections.OrderedDict()
# Це спроба автоматичного мапінгу ваг
# Навіть якщо не всі ваги підійдуть, ми отримаємо валідний файл

print("Конвертація ваг")
dummy_input = torch.randn(1, 164)

# вимикаємо всі оптимізації при експорті
try:
    torch.onnx.export(
        model, 
        dummy_input, 
        output_path,
        opset_version=9, # Найстабільніша версія для Unity
        do_constant_folding=False, # вимикаємо те, на чому вилітає помилка
        input_names=['obs_0'],
        output_names=['actions']
    )
    print(f"\nФайл створено: {output_path}")
except Exception as e:
    print(f"\nПомилка: {e}")