# 🎮 FIAP Cloud Games – Payments API

API responsável pelo **processamento de pagamentos** no ecossistema **FIAP Cloud Games**, centralizando as regras de negócio de cobrança, validação e persistência dos pagamentos, além de integração segura com outros serviços via **Azure API Management (APIM)**.

---

## 🚀 Tech Challenge – FIAP (Fase 3)

Este projeto faz parte do **Tech Challenge** do curso de pós-graduação em **Arquitetura de Sistemas .NET**, aplicando conceitos de **microsserviços**, **segurança**, **integração via API Gateway** e **processamento assíncrono**.

---

## 🧩 Visão Geral da Solução

A **Payments API** é um microsserviço independente, responsável exclusivamente pelo domínio de pagamentos.

---

## 🏗️ Arquitetura do Microsserviço
O projeto está organizado em camadas (DDD) contendo os seguintes projetos:
- **FCG.Payments.API** — Expõe endpoints e recebe requisições do cliente.
- **FCG.Payments.Service** — Executa regras de negócio e casos de uso.
- **FCG.Payments.Domain** — Define o modelo e as regras centrais do negócio.
- **FCG.Payments.Infrastructure** — Implementa persistência e integrações externas.
  
---

## 🔄 Fluxo de Processamento de Pagamentos

1 → Um pedido é criado na API de Games / Orders  
2 → O evento de pedido é publicado no Azure Service Bus  
3 → A Azure Function consome o evento  
4 → A Function chama a Payments API via APIM  
5 → A Payments API:
   - Valida o pedido
   - Processa o pagamento
   - Aprova ou recusa conforme regras de negócio
   - Persiste o resultado no banco de dados

---

## 📌 Responsabilidades da Payments API

- 💳 Processar pagamentos de pedidos
- ✅ Validar dados de pagamento
- 💾 Persistir histórico de pagamentos
- 📄 Disponibilizar endpoints para consulta de pagamentos

---

## 🛠️ Tecnologias Utilizadas
- ⚙️ **Runtime** — [.NET 8 (C#)](https://dotnet.microsoft.com/download/dotnet/8.0)
- 🔐 **Segurança** — [JWT Bearer Authentication](https://jwt.io/)
- 🐘 **Persistência** — [Entity Framework Core](https://learn.microsoft.com/ef/) e [PostgreSQL](https://www.postgresql.org)
- 🧱 **Validação** — [FluentValidation](https://fluentvalidation.net/)
- 🐳 **Conteinerização** — [Docker](https://www.docker.com)

---

## 🐳 Execução via Docker (Local)
```bash
#Build da imagem
docker build -t fcg-payments-api:latest .

#Executar container
docker run -d --name fcg-payments-local -p 8080:8080 \
-e ConnectionStrings__FCG="Sua-String-Conexao" \
-e Jwt__Key="Seu-Segredo-JWT" \
fcg-payments-api:latest
```

