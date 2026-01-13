# 📘 Projeto — Documentação Geral

Desafio de um projeto Full-Stack - Overview das tecnologias e funcionalidades do projeto.

---

## 🚀 Funcionalidades do Projeto

### 📄 **Documentação com OpenAPI usando Scalar**
- Documentação interativa da API utilizando **Scalar**.
- Permite testar endpoints diretamente pela interface.
- Configuração de segurança para autenticação via JWT.
- Interface intuitiva e simples de usar

---

### 🔐 **Autenticação e Autorização com JWT**
- Implementação de autenticação baseada em **JSON Web Tokens (JWT)**.
- Suporte a controle de acesso via **Claims** e **Roles**.
- Middleware configurado para validação automática dos tokens.
- Função para adquirir **userId** automaticamente para cada Endpoint


### 🗄️ **Banco de Dados SQL Server**
- Conexão com **SQL Server**.
- Utilização do micro Mapeador Objeto-Relacional  **DAPPER**
- Implementação com **Entity Framework Core**.

---

### 🌐 **Política de CORS Customizada**
- Restrição de origens, métodos e cabeçalhos.
- Suporte para ambientes especificos.

### 🧩 **Exceções e Erros Customizados**
- Respostas padronizadas e rastreamento de exceções.

### ✔️ **Validação com FluentValidation**
- Validação de Requests utilizando **FluentValidation**.
- Regras centralizadas e respostas padronizadas.

---

### 📦 **Estruturas de Dados**
- **Entity**: Modelos de banco de dados.
- **Request**: DTOs para entrada de dados.
- **Response**: DTOs para saída de dados.

---

### 🧩 **Arquitetura: Controller, Service, Repository**
- Controllers.
- Regras de negócio isoladas em Services.
- Persistência desacoplada via Repository Pattern.

### 📬 **Resposta de API Customizada**
- Estrutura consistente de retorno:
  - Sucesso
  - Mensagens
  - Erros
  - Dados


## 🧷 Estrutura do Projeto (REST)
```
Back/
  Controller/
  Interface/
    IServices/
    IRespository/
  Services/
  Repository/
  Models/
    Request/
    Response/
    Validator/
    Entity

```

---

## 🏁 Como Executar o Projeto
```sh
dotnet restore
dotnet build
dotnet run
```
Execute a migração através do console do Gerenciador de Pacotes
```sh
Add-Migration <nome-migracação>
Update-Database

```
---

## Futuras funcionalidade:
- Paginação para as entidades restantes
- Adição do Aspire e telemetria
- Logs estruturados

---

## 📄 Licença
Uso FREE, projetos pessoais e uso comercial.

