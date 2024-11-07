# De Mol 2018 - Bitcoin Game

## Over

Dit is een spel bedacht voor een (Wie is) de Mol-spel. Het wordt gespeeld met tien spelers, waaronder een Mol. De groep probeert geld te verdienen voor de pot, de Mol probeert -in het geheim- de groep te saboteren.

Het spel wordt gespeeld met tien spelers, die in een kring zitten en allemaal op een eigen apparaat (laptop / tablet) op het spel inloggen. Zij spelen het spel door met elkaar te praten terwijl ze alleen hun eigen scherm kunnen zien. Ze zien dus niet de schermen van de andere spelers.

Er is een spelleider (admin) die het spel kan starten en beëindigen, nieuwe rondes kan starten, en na afloop het resultaat kan inzien.

De spelregels die de spelers te zien krijgen, zijn in [SPELREGELS.md](SPELREGELS.md) te vinden.

## Applicatieoverzicht

De applicatie bestaat uit drie onderdelen:

- Een MSSQL database server;
- Een C# server (DeMol2018.BitcoinGame.GameServer) die met de database communiceert;
- Een React app (DeMol2018.BitcoinGame.ReactApp) die met de server communiceert.

De communicatie tussen applicatie en server vindt plaats met een SignalR-websocket; dit zorgt ervoor dat spelers realtime updates ontvangen.

## Requirements

Om de applicatie te kunnen draaien, zijn de volgende zaken nodig:

- .NET 8 SDK (voor de server)
- Node 20+ en Yarn (voor de React app)
- MSSQL instance, of Docker (zie [deze Docker compose file](develop/docker-compose.yml))

## De applicatie draaien

### Database

Het database schema wordt automatisch opgezet bij de eerste keer dat de server opstart. Van tevoren moet er wel eerst een database aangemaakt worden. Na het opzetten van het schema dien je handmatig de tabellen te vullen met gegevens om een spel te kunnen starten (automatische scripts hiervoor zijn TODO).

### Server

Zorg eerst voor een lokale versie van `appsettings.json`, door de example file `appsettings.Development.json` te kopiëren. Pas in deze file de connection string voor de database aan, indien nodig.

De server start je vervolgens als volgt:

```
cd DeMol2018.BitcoinGame.GameServer
dotnet build
dotnet run
```

De applicatie draait vervolgens standaard op http://localhost:5000 (ook aan te passen in `appsettings.json`).

### Client app

De client app heeft ook een environment file nodig, `.env`. Deze is te kopiëren vanaf `.env.example`. De enige benodigde configuratie is de URL van de server.

De applicatie start je vervolgens als volgt.

Ga naar de map van de applicatie:

```
cd DeMol2018.BitcoinGame.ReactApp
```

Installeer de benodigde packages:

```
yarn
```

Om de applicatie te starten in development mode (met hot reload):

```
yarn start
```

Om een productiebuild te maken:

```
yarn build
```

Om de linter te draaien:

```
yarn lint
```