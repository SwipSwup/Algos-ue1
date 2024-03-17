# Aktienkursverwaltung

Dieses Programm ermöglicht die Verwaltung von Aktienkursen mithilfe einer Hashtabelle. Jede Aktie wird mit ihrem Namen, der Wertpapierkennnummer (WKN) als String und einem Kürzel gespeichert. Zusätzlich werden die Kursdaten der vergangenen 30 Tage für jede Aktie verwaltet, einschließlich (Datum, Eröffnungskurs, Höchstkurs, Tiefstkurs, Schlusskurs, Volumen, Adjusted Close).

## Funktionalitäten

Das Programm bietet die folgenden Funktionen:

- **ADD**: Fügt eine neue Aktie mit Namen, WKN und Kürzel hinzu.
- **DEL**: Löscht eine Aktie aus der Verwaltung.
- **IMPORT**: Importiert Kursdaten aus einer CSV-Datei für eine Aktie.
- **SEARCH**: Sucht nach einer Aktie in der Hashtabelle (durch Eingabe von Namen oder Kürzel) und gibt die aktuellsten Kursdaten aus.
- **PLOT**: Gibt die Schlusskurse der letzten 30 Tage einer Aktie als ASCII-Grafik aus.
- **SAVE <filename>**: Speichert die Hashtabelle in einer Datei ab.
- **LOAD <filename>**: Lädt die Hashtabelle aus einer Datei.
- **QUIT**: Beendet das Programm.

## Verwendung

Um das Programm zu nutzen, können Sie die folgenden Schritte ausführen:

1. Führen Sie das Programm aus.
2. Wählen Sie eine der oben genannten Optionen aus dem Menü, um die gewünschte Aktion auszuführen.
3. Befolgen Sie die Anweisungen des Programms für weitere Interaktionen.

## Beispiel

```bash
$ ./aktienverwaltung
Wähle eine Aktion:
1. ADD
2. DEL
3. IMPORT
4. SEARCH
5. PLOT
6. SAVE <filename>
7. LOAD <filename>
8. QUIT

> 1
Name der Aktie: Apple Inc.
ISN: AAPL123456
Symbol: AAPL
Aktie wurde hinzugefügt.

Wähle eine Aktion:
1. ADD
2. DEL
3. IMPORT
4. SEARCH
5. PLOT
6. SAVE <filename>
7. LOAD <filename>
8. QUIT

> 3
Dateipfad der CSV-Datei: apple_data.csv
Kursdaten wurden importiert.

...

