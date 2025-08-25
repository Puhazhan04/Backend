# Hausaufgabe 1
Entwickelt einen AddressController welcher die HTTP-Methode `GET` implementiert.

Dabei sind folgende Anfragen möglich:
- `GetAllAddresses` (Endpunkt `/address`)
- `GetById` (Endpunkt `/address/<id>`)

Die Antworten sollen jeweils auch die Stadt und das Land enthalten. Hierzu ist ein passendes DTO pro Entität zu entwickeln
- AddressResponseDto
- BaseCityResponseDto
- BaseCountryResponseDto

Der AddressService (auch zu entwickeln) soll mit der Datenbank über Entities kommunizieren und die DTOs als Rückgabewert der Methoden beinhalten.