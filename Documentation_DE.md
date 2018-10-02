# Dokumentation TextRpgCreator
Dies ist die Dokumentation für den TextRpgCreator.
Diese Datei ist im Markdown-Format geschrieben. Ich empfehle einen Editor zu verwenden, der Ihnen die Überschriften usw. markiert.

## Programm starten
Das Programm wurde in C# unter Verwendung von .NET Core entwickelt. Als Betriebssystem wird nur Linux unterstützt, theoretisch ist es jedoch möglich die Anwendung auf Windows auszuführen.

Wie Sie die .NET Core Runtime installieren können, lesen sie auf der [offiziellen Seite von Microsoft](https://www.microsoft.com/net/download). Falls Sie das Projekt auch kompilieren wollen sollten Sie jedoch das SDK installieren.

Zum Kompilieren und Starten: im Ordner TextRpgMaker: `$ dotnet run`

Zum Starten der kompilierten Version: im Ordner TextRpgMaker/bin/Debug/netcoreapp2.0: 'dotnet TextRpgMaker.dll'

## Projekt laden und Spiel starten
Es sollte sich ein Fenster mit Menüleiste öffnen. Unter Project->Load können Sie ein Projekt laden. Um das mitgelieferte kleine Beispiel zu öffnen, wählen Sie den Ordner ExampleProject im Stammverzeichnis des Projekts aus.
Es sollte sich eine TextBox mit dem Text "Project loaded" öffnen. Nun kann über Game->Start new ein neues Spiel gestartet werden.

## Spieloberfläche
Sie kommen nun zur Charakterauswahl. In der Mitte der Oberfläche befindet sich die Ausgabe. Hier sollten jetzt die Charaktere mit ihren Eigenschaften aufgelistet sein. Im unteren  Bereich befindet sich die Eingabe. Oftmals wird Ihnen die Möglichkeit gegeben, zwischen mehreren Optionen zu wählen, dann sind diese über ein Dropdown-Menu auszuwählen und mit dem "Enter"-Knopf zu bestätigen.
Sie haben jetzt Ihren Charakter ausgewählt und befinden sich nun im ersten Dialog. 

Wenn Sie nicht mehr in einem Dialog sind werden Sie unten ein Eingabefeld sehen. Geben Sie `help` ein und drücken Enter um die verfügbaren Befehle anzuzeigen. Die einzelnen Befehle finden Sie im nächsten Kapitel.

## Befehle
Da Markdown spitze Klammern als HTML-Tags interpretiert verwende ich für Pflichtparameter runde Klammern ().

- help: zeigt verfügbare Befehle an
- talk (Name der Person): Person ansprechen (Dialog starten)
- take (Name des Items): Ein Item aus der Szene aufnehmen
- lookaround: Die Szene betrachten. Hier sehen Sie verfügbare Charaktere, Items und Verbindungen zu anderen Szenen.
- look: Ein Gegenstand im Inventar oder der Szene bzw. einen Charakter ansehen.
- inventory: listet Inventar auf
- go: Einer Verbindung zu einer anderen Szene folgen.




