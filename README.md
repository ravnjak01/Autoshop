
# 📘 Autoshop

Ovo je web aplikacija razvijena pomoću **.NET Core Web API** za backend i **Angular** za frontend.  
Aplikacija služi za kupovinu proizvoda za automobile (gume, akumulatori, kozmetika, itd.)


## ⚙️ Tehnologije i preduvjeti

Prije pokretanja aplikacije potrebno je instalirati:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)
- SQL Server

---

## 🔧 Backend – Pokretanje API servera

### 1. Pokretanje API - ja

Potrebno je postaviti RS1_2024_25.API kao **Startup Projekat**.
Pokretanjem projekta(API - ja), kreira se baza auto-shop, moguće je postaviti i drugačiji način baze u appsettingsu, jer se tu nalazi konekcion string.
Osim kreiranja baze, izvrašavaju se i sve migracione skripte, gdje se kreiraju sve tabele.

Link na koji se pokreće API: http://localhost:7000/swagger/index.html


### 2. Seeder podaci

Seed se izvršava na endpointu http://localhost:7000/data-seed a isto tako se može provjeriti da count 
svake dodane tabele sa http://localhost:7000/data-seed.

### 3. Ažuriranje baze

Kada se doda neka nova migraciona skripta, to jeste ažurira se baza nije potrebno ručno uraditi 
update baze, samo je potrebno pokrenuti API i update će se izvršiti.

## 💻 Frontend – Angular aplikacija

### 1. Instalacija zavisnosti
```bash

cd frontend
npm install
```

### 2. Pokretanje aplikacije

```bash

ng serve
```

### 3. Konfiguracija API URL-a

U `src/environments/environment.ts`:

```ts
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7100/api'
};
```

---

## 🔐 Test login podaci


- Da li je aktivan 2FA za korisnike iz seed podataka
- Da li se lozinke generišu automatski (random) ili statično.
  Ako su statične, onda treba ih navesti.

| Uloga     | Username      | Lozinka     |
|-----------|---------------|--------------|
| Admin     | admin         | Admin123!    |

---

## 📄 Autor

Ime i prezime:
Anesa Atlić Softić

Godina studija: 3

Predmet: Razvoj softvera I
