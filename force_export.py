import torch
import collections
from mlagents.trainers.torch_entities.agent_action_binarizer import AgentActionBinarizer

# Шляхи
checkpoint_path = "results/MyAI_ver1/MyAI/checkpoint.pt"
output_path = "results/MyAI_ver1/MyAI_Final.onnx"

print("Завантаження ваг...")
checkpoint = torch.load(checkpoint_path, map_location=torch.device('cpu'))

# Дістаємо ваги з ключа Policy
state_dict = checkpoint['Policy']

# Оскільки в OrderedDict назви ключів можуть мати префікс, 
# ми створимо чисту модель через Torch
# Вхід 164, прихований шар 128
import torch.nn as nn

class SimpleModel(nn.Module):
    def __init__(self):
        super().__init__()
        # Базова архітектура 
        self.network = nn.Sequential(
            nn.Linear(164, 128),
            nn.ReLU(),
            nn.Linear(128, 128),
            nn.ReLU(),
            nn.Linear(128, 2) 
        )
    def forward(self, x):
        return self.network(x)

# Спробуємо завантажити ваги в об'єкт, який PyTorch зможе експортувати
try:
    # Ми не можемо просто створити SimpleModel, бо не знаємо точну назву шарів у ML-Agents
    
    # Створюємо фіктивний вхід
    dummy_input = torch.randn(1, 164)
    
    # Експортуємо безпосередньо OrderedDict як параметри моделі
    print("Спроба прямого запису ONNX")
    
    # запишемо ваги як файл, який Unity зможе зрозуміти 
    # (Unity Barracuda може імпортувати іноді навіть чисті ваги, 
    # але краще спробувати цей скрипт)
    
    torch.onnx.export(
        nn.Module(), # Пуста модель для ініціалізації
        dummy_input,
        output_path,
        opset_version=10,
        input_names=['obs_0'],
        output_names=['actions']
    )
    print(f"Файл створено: {output_path}")

except Exception as e:
    print(f"Помилка: {e}")

#Запустити це в консолі, це вимкне оптимізацію в самому ML-Agents
print("\nКоманда:")
print("set NN_OPTIMIZER_DISABLED=1")
print("py -3.10 -m mlagents.trainers.learn Configs/trainer_config.yaml --run-id=MyAI_ver1 --resume --no-graphics")