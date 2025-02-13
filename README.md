# BookService
## 1. Autorzy (AuthorController)
### 1.1. Dodaj Autora

Metoda: POST /api/author

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Request Body:


``` json
{
  "FirstName": "Jan",
  "LastName": "Kowalski",
  "Country": "Polska",
  "YearOfBirth": 1980
}
```

Response (201 Created):
``` json

{
  "Id": 1
}
```
### 1.2. Pobierz Wszystkich Autorów

Metoda: GET /api/author

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Query Params:

authorName (string, opcjonalnie): Filtr po imieniu lub nazwisku autora.

showRemoved (bool, opcjonalnie): Czy pokazywać usuniętych autorów.

pageSize (int, opcjonalnie): Liczba autorów na stronę (domyślnie 50).

pageNumber (int, opcjonalnie): Numer strony (domyślnie 1).

Response (200 OK):
``` json

{
  "ItemsCount": 10,
  "TotalItemsCount": 100,
  "PageNumber": 1,
  "PageSize": 10,
  "Items": [
    {
      "Id": 1,
      "FirstName": "Jan",
      "LastName": "Kowalski",
      "Country": "Polska",
      "YearOfBirth": 1980,
      "IsRemoved": false
    }
  ]
}
```
### 1.3. Pobierz Autora po ID

Metoda: GET /api/author/{authorId}

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Path Params:

authorId (int): ID autora.

Response (200 OK):
``` json

{
  "Id": 1,
  "FirstName": "Jan",
  "LastName": "Kowalski",
  "Country": "Polska",
  "YearOfBirth": 1980,
  "IsRemoved": false
}
```
### 1.4. Usuń Autora (Soft Delete)

Metoda: DELETE /api/author/{authorId}

Autoryzacja: Wymaga roli Admin.

Path Params:

authorId (int): ID autora.

Response (204 No Content): Brak treści.

### 1.5. Edytuj Autora

Metoda: PUT /api/author/{authorId}

Autoryzacja: Wymaga roli Admin.

Path Params:

authorId (int): ID autora.

Request Body:
``` json

{
  "FirstName": "Jan",
  "LastName": "Nowak",
  "Country": "Polska",
  "YearOfBirth": 1985
}
```
Response (200 OK): Brak treści.

## 2. Książki (BookController)
### 2.1. Dodaj Książkę

Metoda: POST /api/book

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Request Body:
``` json

{
  "Title": "Władca Pierścieni",
  "AuthorsIds": [1, 2]
}
```
Response (201 Created):
``` json

{
  "Id": 1
}
```
### 2.2. Pobierz Wszystkie Książki

Metoda: GET /api/book

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Query Params:

pageSize (int, opcjonalnie): Liczba książek na stronę (domyślnie 50).

pageNumber (int, opcjonalnie): Numer strony (domyślnie 1).

showRemoved (bool, opcjonalnie): Czy pokazywać usunięte książki.

includeBookAuthors (bool, opcjonalnie): Czy dołączyć szczegóły autorów.

title (string, opcjonalnie): Filtr po tytule książki.

authorId (int, opcjonalnie): Filtr po ID autora.

Response (200 OK):
``` json

{
  "ItemsCount": 10,
  "TotalItemsCount": 100,
  "PageNumber": 1,
  "PageSize": 10,
  "Items": [
    {
      "Id": 1,
      "Title": "Władca Pierścieni",
      "Authors": [
        {
          "Id": 1,
          "FirstName": "Jan",
          "LastName": "Kowalski",
          "Country": "Polska",
          "YearOfBirth": 1980,
          "IsRemoved": false
        }
      ],
      "IsRemoved": false
    }
  ]
}
```
### 2.3. Pobierz Książkę po ID

Metoda: GET /api/book/{bookId}

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Path Params:

bookId (int): ID książki.

Query Params:

includeBookAuthors (bool, opcjonalnie): Czy dołączyć szczegóły autorów.

Response (200 OK):
``` json

{
  "Id": 1,
  "Title": "Władca Pierścieni",
  "Authors": [
    {
      "Id": 1,
      "FirstName": "Jan",
      "LastName": "Kowalski",
      "Country": "Polska",
      "YearOfBirth": 1980,
      "IsRemoved": false
    }
  ],
  "IsRemoved": false
}
```
### 2.4. Usuń Książkę (Soft Delete)

Metoda: DELETE /api/book/{bookId}

Autoryzacja: Wymaga roli Admin.

Path Params:

    bookId (int): ID książki.

Response (204 No Content): Brak treści.

### 2.5. Edytuj Książkę

Metoda: PUT /api/book/{bookId}

Autoryzacja: Wymaga roli Admin.

Path Params:

bookId (int): ID książki.

Request Body:
``` json

{
  "Title": "Hobbit",
  "AuthorsIds": [1]
}
```
Response (200 OK): Brak treści.

## 3. Punkty Wymiany Książek (BookPointController)
### 3.1. Dodaj Punkt Wymiany Książek

Metoda: POST /api/bookpoint

Autoryzacja: Wymaga roli Admin.

Request Body:
``` json

{
  "Region": 1,
  "Lat": 50.0647,
  "Long": 19.9450,
  "Capacity": 100
}
```
Response (201 Created):
``` json

{
  "Id": 1
}
```
### 3.2. Pobierz Wszystkie Punkty Wymiany Książek

Metoda: GET /api/bookpoint

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Query Params:

pageSize (int, opcjonalnie): Liczba punktów na stronę (domyślnie 50).

pageNumber (int, opcjonalnie): Numer strony (domyślnie 1).

region (int, opcjonalnie): Filtr po regionie.

Response (200 OK):
``` json

{
  "ItemsCount": 10,
  "TotalItemsCount": 100,
  "PageNumber": 1,
  "PageSize": 10,
  "Items": [
    {
      "Id": 1,
      "Lat": 50.0647,
      "Long": 19.9450,
      "Region": 1,
      "Capacity": 100
    }
  ]
}
```
### 3.3. Pobierz Punkt Wymiany Książek po ID

Metoda: GET /api/bookpoint/{bookPointId}

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Path Params:

    bookPointId (int): ID punktu wymiany.

Response (200 OK):
``` json

{
  "Id": 1,
  "Lat": 50.0647,
  "Long": 19.9450,
  "Region": 1,
  "Capacity": 100
}
```
### 3.4. Usuń Punkt Wymiany Książek (Soft Delete)

Metoda: DELETE /api/bookpoint/{bookPointId}

Autoryzacja: Wymaga roli Admin.

Path Params:

bookPointId (int): ID punktu wymiany.

Response (204 No Content): Brak treści.

### 3.5. Edytuj Punkt Wymiany Książek

Metoda: PUT /api/bookpoint/{bookPointId}

Autoryzacja: Wymaga roli Admin.

Path Params:

    bookPointId (int): ID punktu wymiany.

Request Body:
``` json

{
  "Region": 2,
  "Lat": 51.0647,
  "Long": 20.9450,
  "Capacity": 150
}
```
Response (200 OK): Brak treści.

## 4. Wymiany Książek (ExchangesController)
### 4.1. Pobierz Wszystkie Wymiany (Admin)

Metoda: GET /api/exchanges/admin

Autoryzacja: Wymaga roli Admin.

Query Params:

pageSize (int, opcjonalnie): Liczba wymian na stronę (domyślnie 50).

pageNumber (int, opcjonalnie): Numer strony (domyślnie 1).

initiatorUserId (int, opcjonalnie): Filtr po ID użytkownika inicjującego wymianę.

receiverUserId (int, opcjonalnie): Filtr po ID użytkownika otrzymującego wymianę.

status (ExchangeStatus, opcjonalnie): Filtr po statusie wymiany.

Response (200 OK):
``` json

{
  "ItemsCount": 10,
  "TotalItemsCount": 100,
  "PageNumber": 1,
  "PageSize": 10,
  "Items": [
    {
      "Initiator": {
        "UserId": 1,
        "UserFirstName": "Jan",
        "UserLastName": "Kowalski",
        "BookItemId": 1,
        "BookTitle": "Władca Pierścieni",
        "BookImageId": 1
      },
      "Receiver": {
        "UserId": 2,
        "UserFirstName": "Anna",
        "UserLastName": "Nowak",
        "BookItemId": 2,
        "BookTitle": "Hobbit",
        "BookImageId": 2
      },
      "Status": "Pending",
      "ExchangeId": 1
    }
  ]
}
```
### 4.2. Pobierz Wymiany dla Użytkownika

Metoda: GET /api/exchanges

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Query Params:

pageSize (int, opcjonalnie): Liczba wymian na stronę (domyślnie 50).

pageNumber (int, opcjonalnie): Numer strony (domyślnie 1).

Response (200 OK):
``` json

{
  "ItemsCount": 10,
  "TotalItemsCount": 100,
  "PageNumber": 1,
  "PageSize": 10,
  "Items": [
    {
      "Initiator": {
        "UserId": 1,
        "UserFirstName": "Jan",
        "UserLastName": "Kowalski",
        "BookItemId": 1,
        "BookTitle": "Władca Pierścieni",
        "BookImageId": 1
      },
      "Receiver": {
        "UserId": 2,
        "UserFirstName": "Anna",
        "UserLastName": "Nowak",
        "BookItemId": 2,
        "BookTitle": "Hobbit",
        "BookImageId": 2
      },
      "Status": "Pending",
      "ExchangeId": 1
    }
  ]
}
```
### 4.3. Dodaj Wymianę

Metoda: POST /api/exchanges

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Request Body:
``` json

{
  "InitiatorBookItemId": 1,
  "ReceiverUserId": 2,
  "ReceiverBookItemId": 2
}
```
Response (201 Created): Brak treści.

4.4. Odpowiedz na Wymianę

Metoda: PUT /api/exchanges

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Request Body:
``` json

{
  "ExchangeId": 1,
  "Accepted": true
}
```
Response (200 OK): Brak treści.

## 5. Dopasowania (MatchController)
### 5.1. Pobierz Dopasowania dla Użytkownika

Metoda: GET /api/match

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Query Params:

    pageSize (int, opcjonalnie): Liczba dopasowań na stronę (domyślnie 50).

    pageNumber (int, opcjonalnie): Numer strony (domyślnie 1).

Response (200 OK):
``` json

{
  "Matches": [
    {
      "UserId": 2,
      "FirstName": "Anna",
      "LastName": "Nowak",
      "MatchBookPair": [
        {
          "OfferedBook": {
            "UserBookItemId": 1,
            "Title": "Władca Pierścieni",
            "ImageId": 1
          },
          "RequestedBook": {
            "UserBookItemId": 2,
            "Title": "Hobbit",
            "ImageId": 2
          },
          "ExchangeId": 1,
          "ExchangeStatus": "Pending",
          "IsMyOffer": true
        }
      ]
    }
  ]
}
```
## 6. Książki Użytkownika (UserBookItemController)
### 6.1. Dodaj Obraz do Książki Użytkownika

Metoda: POST /api/userbookitem/image

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Request Body (FormData):

file (IFormFile): Plik obrazu.

Response (201 Created):
``` json

{
  "ImageId": 1
}
```
### 6.2. Pobierz Obraz

Metoda: GET /api/userbookitem/image/{imageId}

Autoryzacja: Brak (dostęp publiczny).

Path Params:

imageId (int): ID obrazu.

Response (200 OK): Zwraca plik obrazu.

### 6.3. Dodaj Książkę Użytkownika

Metoda: POST /api/userbookitem

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Request Body:
``` json

{
  "BookReferenceId": 1,
  "BookPointId": 1,
  "Description": "Moja ulubiona książka",
  "Status": "ActivePublic",
  "ImageId": 1
}
```
Response (201 Created):
``` json

{
  "Id": 1
}
```
### 6.4. Pobierz Wszystkie Książki Użytkownika (Admin)

Metoda: GET /api/userbookitem

Autoryzacja: Wymaga roli Admin.

Query Params:

  pageSize (int, opcjonalnie): Liczba książek na stronę (domyślnie 50).

  pageNumber (int, opcjonalnie): Numer strony (domyślnie 1).

  includeBookAuthors (bool, opcjonalnie): Czy dołączyć szczegóły autorów.

  itemStatus (UserBookItemStatus, opcjonalnie): Filtr po statusie książki.

  region (Region, opcjonalnie): Filtr po regionie.

  userId (int, opcjonalnie): Filtr po ID użytkownika.

  title (string, opcjonalnie): Filtr po tytule książki.

  startDate (DateTime, opcjonalnie): Filtr po dacie rozpoczęcia.

  endDate (DateTime, opcjonalnie): Filtr po dacie zakończenia.

Response (200 OK):
``` json

{
  "ItemsCount": 10,
  "TotalItemsCount": 100,
  "PageNumber": 1,
  "PageSize": 10,
  "Items": [
    {
      "Id": 1,
      "UserId": 1,
      "Status": "ActivePublic",
      "Description": "Moja ulubiona książka",
      "ImageId": 1,
      "BookReference": {
        "Id": 1,
        "Title": "Władca Pierścieni",
        "Authors": [
          {
            "Id": 1,
            "FirstName": "Jan",
            "LastName": "Kowalski",
            "Country": "Polska",
            "YearOfBirth": 1980,
            "IsRemoved": false
          }
        ],
        "IsRemoved": false
      }
    }
  ]
}
```
### 6.5. Pobierz Książki Użytkownika

Metoda: GET /api/userbookitem/user-books/{userId}

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Path Params:

  userId (int): ID użytkownika.

Query Params:

  pageSize (int, opcjonalnie): Liczba książek na stronę (domyślnie 50).

  pageNumber (int, opcjonalnie): Numer strony (domyślnie 1).

  includeBookAuthors (bool, opcjonalnie): Czy dołączyć szczegóły autorów.

  title (string, opcjonalnie): Filtr po tytule książki.

Response (200 OK):
``` json

{
  "ItemsCount": 10,
  "TotalItemsCount": 100,
  "PageNumber": 1,
  "PageSize": 10,
  "Items": [
    {
      "Id": 1,
      "UserId": 1,
      "Status": "ActivePublic",
      "Description": "Moja ulubiona książka",
      "ImageId": 1,
      "BookReference": {
        "Id": 1,
        "Title": "Władca Pierścieni",
        "Authors": [
          {
            "Id": 1,
            "FirstName": "Jan",
            "LastName": "Kowalski",
            "Country": "Polska",
            "YearOfBirth": 1980,
            "IsRemoved": false
          }
        ],
        "IsRemoved": false
      }
    }
  ]
}
```
### 6.6. Pobierz Feed Książek

Metoda: GET /api/userbookitem/feed

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Query Params:

pageSize (int, opcjonalnie): Liczba książek na stronę (domyślnie 50).

pageNumber (int, opcjonalnie): Numer strony (domyślnie 1).

Response (200 OK):
``` json

{
  "ItemsCount": 10,
  "TotalItemsCount": 100,
  "PageNumber": 1,
  "PageSize": 10,
  "Items": [
    {
      "Id": 1,
      "UserId": 1,
      "Status": "ActivePublic",
      "Description": "Moja ulubiona książka",
      "ImageId": 1,
      "BookReference": {
        "Id": 1,
        "Title": "Władca Pierścieni",
        "Authors": [
          {
            "Id": 1,
            "FirstName": "Jan",
            "LastName": "Kowalski",
            "Country": "Polska",
            "YearOfBirth": 1980,
            "IsRemoved": false
          }
        ],
        "IsRemoved": false
      }
    }
  ]
}
```
### 6.7. Pobierz Książkę Użytkownika po ID

Metoda: GET /api/userbookitem/{userBookItemId}

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Path Params:

userBookItemId (int): ID książki użytkownika.

Response (200 OK):
``` json

{
  "Id": 1,
  "UserId": 1,
  "Status": "ActivePublic",
  "Description": "Moja ulubiona książka",
  "ImageId": 1,
  "BookReference": {
    "Id": 1,
    "Title": "Władca Pierścieni",
    "Authors": [
      {
        "Id": 1,
        "FirstName": "Jan",
        "LastName": "Kowalski",
        "Country": "Polska",
        "YearOfBirth": 1980,
        "IsRemoved": false
      }
    ],
    "IsRemoved": false
  }
}
```
### 6.8. Usuń Książkę Użytkownika

Metoda: DELETE /api/userbookitem/{userBookItemId}

Autoryzacja: Wymaga zalogowanego użytkownika (rola User) lub roli Admin.

Path Params:

userBookItemId (int): ID książki użytkownika.

Response (204 No Content): Brak treści.

### 6.9. Edytuj Książkę Użytkownika

Metoda: PUT /api/userbookitem/{userBookItemId}

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Path Params:

userBookItemId (int): ID książki użytkownika.

Request Body:
``` json

{
  "Description": "Zaktualizowany opis",
  "Status": "ActivePrivate",
  "BookReferenceId": 1,
  "BookPointId": 1,
  "ImageId": 1
}
```
Response (200 OK): Brak treści.

### 6.10. Zmień Region dla Wszystkich Książek Użytkownika

Metoda: PUT /api/userbookitem/batch-change-region

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Response (200 OK): Brak treści.

### 6.11. Zmień Status Książki Użytkownika (Admin)

Metoda: PUT /api/userbookitem/{userBookItemId}/change-status

Autoryzacja: Wymaga roli Admin.

Path Params:

userBookItemId (int): ID książki użytkownika.

Request Body:
``` json

{
  "Status": "Disabled"
}
```
Response (200 OK): Brak treści.

### 6.12. Dodaj/Usuń Polubienie Książki

Metoda: POST /api/userbookitem/toggle-like

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Request Body:
```json

{
  "UserBookItemId": 1
}
```
Response (200 OK):
``` json

{
  "HaveNewMatches": true
}
```
### 6.13. Pobierz Polubienia Użytkownika

Metoda: GET /api/userbookitem/get-like

Autoryzacja: Wymaga zalogowanego użytkownika (rola User).

Query Params:

    pageSize (int, opcjonalnie): Liczba polubień na stronę (domyślnie 50).

    pageNumber (int, opcjonalnie): Numer strony (domyślnie 1).

Response (200 OK):
``` json

{
  "UserId": 1,
  "UserLikes": {
    "ItemsCount": 10,
    "TotalItemsCount": 100,
    "PageNumber": 1,
    "PageSize": 10,
    "Items": [
      {
        "Id": 1,
        "UserId": 1,
        "Status": "ActivePublic",
        "Description": "Moja ulubiona książka",
        "ImageId": 1,
        "BookReference": {
          "Id": 1,
          "Title": "Władca Pierścieni",
          "Authors": [
            {
              "Id": 1,
              "FirstName": "Jan",
              "LastName": "Kowalski",
              "Country": "Polska",
              "YearOfBirth": 1980,
              "IsRemoved": false
            }
          ],
          "IsRemoved": false
        }
      }
    ]
  }
}
```
## 7 Schemat bazy danych 
![obraz](https://github.com/user-attachments/assets/a2cd0fed-0e8a-4ce8-a14b-a49a0a0b43d7)


# AccountService
## 1. AdminController

Endpointy związane z zarządzaniem administratorami. Wymagają roli Admin (z wyjątkiem logowania).
### 1.1. Rejestracja administratora

Metoda: POST /api/Admin/Register

Autoryzacja: Wymaga roli Admin.

Request Body:
``` json
{
  "Email": "admin@example.com",
  "FirstName": "Jan",
  "LastName": "Kowalski",
  "Password": "haslo123"
}
```
Response:

201 Created: Pomyślna rejestracja.

400 Bad Request: Błąd w danych wejściowych.

500 Internal Server Error: Błąd serwera.

### 1.2. Logowanie administratora

Metoda: POST /api/Admin/Login

Autoryzacja: Brak (dostępne dla wszystkich).

Request Body:
``` json

{
  "Email": "admin@example.com",
  "Password": "haslo123"
}
```
Response:

    201 Created: Zwraca tokeny.
 ```   json

    {
      "Token": "jwt-token",
      "RefreshToken": "refresh-token"
    }
```
    400 Bad Request: Błąd w danych wejściowych.

    401 Unauthorized: Nieprawidłowe dane logowania.

### 1.3. Pobierz listę administratorów

Metoda: GET /api/Admin

Autoryzacja: Wymaga roli Admin.

Query Params:

    pageSize (int, opcjonalnie): Liczba wyników na stronę (domyślnie 50).

    pageNumber (int, opcjonalnie): Numer strony (domyślnie 1).

Response:

    200 OK: Zwraca listę administratorów.
  ```  json

    {
      "ItemsCount": 10,
      "TotalItemsCount": 100,
      "PageNumber": 1,
      "PageSize": 10,
      "Items": [
        {
          "Id": 1,
          "Email": "admin@example.com",
          "FirstName": "Jan",
          "LastName": "Kowalski"
        }
      ]
    }
```
    500 Internal Server Error: Błąd serwera.

### 1.4. Pobierz administratora po ID

Metoda: GET /api/Admin/{userId}

Autoryzacja: Wymaga roli Admin.

Path Params:

userId (int): ID administratora.

Response:

200 OK: Zwraca dane administratora.
``` json
{
  "Id": 1,
  "Email": "admin@example.com",
  "FirstName": "Jan",
  "LastName": "Kowalski"
}
```
404 Not Found: Administrator nie istnieje.

500 Internal Server Error: Błąd serwera.

### 2. AuthController

Endpointy związane z autentykacją i zarządzaniem tokenami.
2.1. Odśwież token

Metoda: POST /api/Auth/refresh

Autoryzacja: Wymaga ważnego refresh tokenu.

Request Body:
``` json

{
  "RefreshToken": "refresh-token"
}
```
Response:

200 OK: Zwraca nowe tokeny.
```  json

{
  "Token": "jwt-token",
  "RefreshToken": "refresh-token"
}
```
401 Unauthorized: Nieprawidłowy refresh token.

### 2.2. Testowe endpointy

GET /api/Auth/OnlyValidToken: Wymaga ważnego tokenu.

GET /api/Auth/OnlyUser: Wymaga roli User.

GET /api/Auth/OnlyAdmin: Wymaga roli Admin.

GET /api/Auth/ExpiredToken: Wymaga ważnego refresh tokenu.

## 3. UserController

Endpointy związane z zarządzaniem użytkownikami.
### 3.1. Rejestracja użytkownika

Metoda: POST /api/User/Register

Autoryzacja: Brak (dostępne dla wszystkich).

Request Body:
``` json

{
  "Email": "user@example.com",
  "FirstName": "Anna",
  "LastName": "Nowak",
  "Password": "haslo123",
  "BirthDate": "1990-01-01",
  "Region": "Europe"
}
```
Response:

    201 Created: Pomyślna rejestracja.

    400 Bad Request: Błąd w danych wejściowych.

### 3.2. Logowanie użytkownika

Metoda: POST /api/User/Login

Autoryzacja: Brak (dostępne dla wszystkich).

Request Body:
```  json

{
  "Email": "user@example.com",
  "Password": "haslo123"
}
```
Response:

    201 Created: Zwraca tokeny.
  ```  json


    {
      "Token": "jwt-token",
      "RefreshToken": "refresh-token"
    }
```
    401 Unauthorized: Nieprawidłowe dane logowania.

### 3.3. Pobierz listę użytkowników

Metoda: GET /api/User

Autoryzacja: Wymaga ważnego tokenu.

Query Params:

    pageSize (int, opcjonalnie): Liczba wyników na stronę (domyślnie 50).

    pageNumber (int, opcjonalnie): Numer strony (domyślnie 1).

    fullName (string, opcjonalnie): Filtruj po imieniu i nazwisku.

    status (AccountStatus, opcjonalnie): Filtruj po statusie konta.

Response:

  200 OK: Zwraca listę użytkowników.
```   json
  {
    "ItemsCount": 10,
    "TotalItemsCount": 100,
    "PageNumber": 1,
    "PageSize": 10,
    "Items": [
      {
        "Id": 1,
        "Email": "user@example.com",
        "FirstName": "Anna",
        "LastName": "Nowak",
        "BirthDate": "1990-01-01",
        "Region": "Europe",
        "Status": "ACTIVE"
      }
    ]
  }
```
  500 Internal Server Error: Błąd serwera.

### 3.4. Pobierz użytkownika po ID

  Metoda: GET /api/User/{userId}

  Autoryzacja: Brak (dostępne dla wszystkich).

Path Params:

    userId (int): ID użytkownika.

Response:

    200 OK: Zwraca dane użytkownika.
  ``` json
    {
      "Id": 1,
      "Email": "user@example.com",
      "FirstName": "Anna",
      "LastName": "Nowak",
      "BirthDate": "1990-01-01",
      "Region": "Europe",
      "Status": "ACTIVE"
    }
```
      404 Not Found: Użytkownik nie istnieje.

### 3.5. Zmiana regionu użytkownika

  Metoda: PUT /api/User/region

  Autoryzacja: Wymaga roli User.

  Request Body:
```  json

  {
    "region": "Asia"
  }
```
  Response:

      200 OK: Pomyślna zmiana regionu.

      400 Bad Request: Błąd w danych wejściowych.

## 4. UserManagmentController

Endpointy związane z zarządzaniem statusem użytkowników.
4.1. Zmiana statusu użytkownika

Metoda: POST /api/UserManagment/account-status

Autoryzacja: Wymaga roli Admin.

Request Body:
``` json

{
  "UserId": 1,
  "Status": "INACTIVE"
}
```
Response:

    200 OK: Pomyślna zmiana statusu.

    400 Bad Request: Błąd w danych wejściowych.

Uwagi ogólne

  Wszystkie endpointy zwracają błędy w formacie:
```  json

  {
    "Description": "Opis błędu"
  }
```
  Autoryzacja odbywa się za pomocą tokenów JWT.

  Role wymagane do dostępu są określone w opisie każdego endpointu.
## 5 Schemat bazy danych
![obraz](https://github.com/user-attachments/assets/312af9ec-c6da-4786-a9f4-3b9ff94ffcf1)

# ReportingService
## 1. Tworzenie przypadku (Case)
### 1.1. Utwórz przypadek (automatycznie)

Metoda: POST /api/CaseAction

Autoryzacja: Brak (dostępne dla wszystkich).

Request Body:
``` json

{
  "UserId": 1,
  "Type": "Book",
  "CaseFields": {
    "Title": "Example Book",
    "Author": "John Doe"
  },
  "ItemId": 123
}
```
Response:

    201 Created: Zwraca ID utworzonego przypadku.
 ``` json

    {
      "Id": 1
    }
```
    400 Bad Request: Błąd w danych wejściowych.

    500 Internal Server Error: Błąd serwera.

### 1.2. Utwórz przypadek (przez użytkownika)

Metoda: POST /api/CaseAction/report

Autoryzacja: Wymaga roli User.

Request Body:
``` json

{
  "UserId": 1,
  "Type": "Book",
  "CaseFields": {
    "Title": "Example Book",
    "Author": "John Doe"
  },
  "ItemId": 123,
  "Notes": "This is a user-reported case."
}
```
Response:

    201 Created: Zwraca ID utworzonego przypadku.
 ```   json

    {
      "Id": 1
    }
```
    400 Bad Request: Błąd w danych wejściowych.

    401 Unauthorized: Brak autoryzacji.

    500 Internal Server Error: Błąd serwera.

## 2. Pobieranie przypadków (Cases)
### 2.1. Pobierz listę przypadków

Metoda: GET /api/CaseAction

Autoryzacja: Wymaga roli Admin.

Query Params:

    pageSize (int, opcjonalnie): Liczba wyników na stronę (domyślnie 50).

    pageNumber (int, opcjonalnie): Numer strony (domyślnie 1).

    caseStatus (CaseStatus, opcjonalnie): Filtruj po statusie przypadku.

    caseType (CaseItemType, opcjonalnie): Filtruj po typie przypadku.

    userId (int, opcjonalnie): Filtruj po ID użytkownika.

Response:

    200 OK: Zwraca listę przypadków.
  ``` json
    {
      "ItemsCount": 10,
      "TotalItemsCount": 100,
      "PageNumber": 1,
      "PageSize": 10,
      "Items": [
        {
          "CaseId": 1,
          "UserId": 1,
          "CaseStatus": "OPEN",
          "CaseType": "Book",
          "ReportType": "AutoReport",
          "ReviewerId": null
        }
      ]
    }
```
    401 Unauthorized: Brak autoryzacji.

    500 Internal Server Error: Błąd serwera.

### 2.2. Pobierz szczegóły przypadku

  Metoda: GET /api/CaseAction/{caseId}

  Autoryzacja: Wymaga roli Admin.

  Path Params:

      caseId (int): ID przypadku.

  Response:

      200 OK: Zwraca szczegóły przypadku.
   ```   json

      {
        "CaseId": 1,
        "UserId": 1,
        "ItemId": 123,
        "CaseStatus": "OPEN",
        "CaseType": "Book",
        "ReportType": "AutoReport",
        "ReportNote": "Auto-created case",
        "ReviewerId": null,
        "keyValues": {
          "Title": "Example Book",
          "Author": "John Doe"
        }
      }
```
      404 Not Found: Przypadek nie istnieje.

      401 Unauthorized: Brak autoryzacji.

      500 Internal Server Error: Błąd serwera.

## 3. Zarządzanie przypadkami (Cases)
### 3.1. Przypisz przypadek do recenzenta

  Metoda: PUT /api/CaseAction/{caseId}/Assign

  Autoryzacja: Wymaga roli Admin.

  Path Params:

      caseId (int): ID przypadku.

  Response:

      200 OK: Przypadek został przypisany do recenzenta.

      400 Bad Request: Błąd w danych wejściowych.

      401 Unauthorized: Brak autoryzacji.

      500 Internal Server Error: Błąd serwera.

### 3.2. Zatwierdź przypadek

  Metoda: PUT /api/CaseAction/{caseId}/Approve

  Autoryzacja: Wymaga roli Admin.

  Path Params:

      caseId (int): ID przypadku.

  Response:

      200 OK: Przypadek został zatwierdzony.

      400 Bad Request: Błąd w danych wejściowych.

      401 Unauthorized: Brak autoryzacji.

      500 Internal Server Error: Błąd serwera.

### 3.3. Odrzuć przypadek

  Metoda: PUT /api/CaseAction/{caseId}/Reject

  Autoryzacja: Wymaga roli Admin.

  Path Params:

      caseId (int): ID przypadku.

  Response:

      200 OK: Przypadek został odrzucony.

      400 Bad Request: Błąd w danych wejściowych.

      401 Unauthorized: Brak autoryzacji.

      500 Internal Server Error: Błąd serwera.

## 4 Schemat bazy danych
![obraz](https://github.com/user-attachments/assets/5929c929-705f-4a57-8b1b-467b6ca67834)


Uwagi ogólne

  Wszystkie endpointy zwracają błędy w formacie:
 ``` json

  {
    "Description": "Opis błędu"
  }
```
  Autoryzacja odbywa się za pomocą tokenów JWT.

  Role wymagane do dostępu są określone w opisie każdego endpointu.

Role wymagane w endpointach:

  Admin: Wymagane do zarządzania przypadkami (pobieranie listy, przypisywanie, zatwierdzanie, odrzucanie).

  User: Wymagane do zgłaszania przypadków przez użytkownika.

  Brak autoryzacji: Tylko do automatycznego tworzenia przypadków.
