# LevelUp Hardware – Backend
API Desenvolvida em .NET para o aplicativo LevelUp Hardware

O **LevelUp Hardware** é um aplicativo criado como projeto da disciplina **UPX V da FACENS**, com foco em criar um app de Hardware, permitindo que usuários explorem produtos, reviews, artigos, e sejam direcionados para lojas parceiras através de links afiliados.

Este repositório contém o **backend** do projeto — uma API REST construída em **C# .NET**, responsável pela autenticação, persistência de dados, gerenciamento de conteúdo e integração com o frontend mobile.

## 🚀 Funcionalidades da API
### 👤 Autenticação & Usuários
- Cadastro e login de usuários
- Autenticação com JWT
- Perfis: usuário comum e administrador

### 📰 Artigos, Reviews e Produtos
- CRUD de produtos, artigos e reviews
- Associação de artigos aos produtos

### 🛒 Links de Compra Afiliados
- Redirecionamento para lojas externas

### 🛠️ Administração
- Painel administrativo via API

## 🏗️ Arquitetura
- C# ASP.NET Core 7+
- Entity Framework Core
- JWT Authentication
- Repository Pattern
- RESTful API

## ⚙️ Como Rodar
```
git clone https://github.com/Otavinhopx/UPXV-LevelUp-Hardware-BackEnd
cd UPXV-LevelUp-Hardware-BackEnd
dotnet restore
dotnet ef database update
dotnet run
```

## 📚 Sobre o Projeto
Projeto acadêmico da disciplina **UPX V – FACENS**.
