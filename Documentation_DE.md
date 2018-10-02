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
- go: Einer Verbindung zu einer anderen Szene folgen

## Besondere Funktionen
- Als 'Warrior' können Sie dem Bürgermeister seinen Schnaps nicht abkaufen, als 'Thief' haben Sie jedoch von Anfang an Geld und eine Gesprächsoption erscheint. Auf diese Weise könnte man auch einen Shop bauen.
- Nach der Annahme der Aufgabe bekommt man ein Schwert vom Bürgermeister, dieses findet man im Inventar.
- Bei der Wache ('Guard') erscheinen beim 'Thief' und 'Archer' ebenfalls zusätzliche Gesprächsoptionen, weil sie bereits eine Waffe im Inventar haben. Diese werden bei der Wahl der Option jedoch nicht aus dem Inventar entfernt.
- Nachdem das Schwert von Bürgermeister angenommen wurde ändert sich sein Dialog.
- Nachdem man der Wache das Schwert zeigt öffnet sie das Tor und eine Verbindung zu 'Fields' wird der Szene hinzugefügt, man kann also jetzt 'Town' verlassen.
- Je nachdem, wie man dem Bürgermeister antwortet endet die Geschichte anders. Man könnte so (durch Veränderung von Szenen, anderen Pfaden usw.) eine sehr komplexe Geschichte bauen.

## Hilfe auf der Oberfläche
Es wurde einige Hilfsfunktionen implementiert.
- Nach dem Start der Anwedung steht ein Hilfstext im Ausgabefenster, wurde ein Projekt geladen ändert dieser sich.
- mit dem Befehl 'help' können verfügbare Befehle angezeigt werden
- Eine Oberfläche für eine Hilfe mit mehreren Seiten wurde geschrieben. Diese lädt die Hilfe aus der Datei creators-help.yaml bzw. players-help.yaml, je nachdem ob man Help->Creators->Help text oder Help->Players->TextRpgCreator help wählt. Zusätzlich kann ein Projekt eine Hilfedatei enthalten, diese wird über Help->Players->Project Help geöffnet.
- Über Help->Creators->Export yaml type documentation kann eine Typdefinition für die verschiedenen Objekte in einer Datei angelegt werden. Mit Hilfe dieser Dokumentation und dem Beispielprojekt sollte es möglich sein, seine eigene Geschichte zu schreiben.

## .typ-Dateien
Um die Projektdateien unterteilen zu können ist es möglich, statt einer .yaml-Datei eine .yaml.typ-Datei zu erstellen. Aus diesen wird dann die jeweilige .yaml-Datei beim Laden generiert.


