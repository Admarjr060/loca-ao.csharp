# 🚗 Locadora de Carros - Web API

## Descrição do Sistema

API REST para gerenciamento de uma locadora de carros. Permite cadastrar veículos e registrar locações, com cálculo automático do valor total incluindo desconto de 10% para locações acima de 7 dias.

## 👥 Integrantes

- [Nome do integrante 1]
- [Nome do integrante 2]
- [Nome do integrante 3]

## 🛠️ Tecnologias

- .NET 8 — Minimal API
- Entity Framework Core 8
- SQLite
- JSON

## ▶️ Instruções de Execução

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)

### Passos

```bash
# 1. Clone o repositório
git clone https://github.com/seu-usuario/LocadoraAPI.git
cd LocadoraAPI

# 2. Instale as ferramentas do EF (se ainda não tiver)
dotnet tool install --global dotnet-ef

# 3. Crie o banco de dados via migrations
dotnet ef migrations add InitialCreate
dotnet ef database update

# 4. Execute o projeto
dotnet run
```

A API ficará disponível em: `http://localhost:5000`

---

## 📋 Funcionalidades Implementadas

### Entidades

- **Carro** — Id, Modelo, Placa, ValorDiaria, Disponivel
- **Locação** — Id, Cliente, Dias, Total, CarroId (FK)

### Regra de Negócio

- Desconto de **10%** aplicado automaticamente em locações com **mais de 7 dias**
- Carro é marcado como **indisponível** ao ser locado e **liberado** ao remover a locação
- **Placa única** — não é possível cadastrar dois carros com a mesma placa

---

## 🔗 Endpoints

### Carros

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/carros` | Lista todos os carros |
| GET | `/carros/{id}` | Busca carro por ID |
| POST | `/carros` | Cadastra novo carro |
| PUT | `/carros/{id}` | Atualiza carro |
| DELETE | `/carros/{id}` | Remove carro |

### Locações

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/locacoes` | Lista todas as locações |
| GET | `/locacoes/{id}` | Busca locação por ID |
| POST | `/locacoes` | Cria nova locação |
| PUT | `/locacoes/{id}` | Atualiza locação |
| DELETE | `/locacoes/{id}` | Remove locação e libera carro |

---

## 📦 Exemplos de JSON

### POST /carros
```json
{
  "modelo": "Honda Civic",
  "placa": "ABC-1234",
  "valorDiaria": 150.00
}
```

### POST /locacoes
```json
{
  "cliente": "João Silva",
  "carroId": 1,
  "dias": 10
}
```
> Resultado: desconto de 10% aplicado → Total = 10 × 150 × 0.9 = **R$ 1350,00**

---

## 📁 Estrutura do Projeto

```
LocadoraAPI/
├── Data/
│   └── AppDbContext.cs
├── Models/
│   ├── Carro.cs
│   └── Locacao.cs
├── Program.cs
├── LocadoraAPI.csproj
└── README.md
```
