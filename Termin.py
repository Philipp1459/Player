import time
from datetime import datetime
import os
import ctypes
import subprocess

def maximize_window():
    """Maximiert das Terminal-Fenster"""
    try:
        # Aktuelle Fenster-ID holen und maximieren
        ctypes.windll.user32.ShowWindow(ctypes.windll.kernel32.GetConsoleWindow(), 3)
    except:
        pass

def show_permission_dialog():
    """Zeigt eine Bestätigung im Terminal"""
    print("=" * 60)
    print("TERMIN MANAGEMENT SYSTEM V1.0")
    print("=" * 60)
    print("\n⚠️  SYSTEMVERÄNDERUNGEN ERFORDERLICH")
    print("\nDas Programm möchte folgende Veränderungen vornehmen:")
    print("  • Systemzeit übernehmen")
    print("  • Systemeinstellungen anpassen")
    print("\nDarfst du diese Veränderungen erlauben?")
    
    while True:
        antwort = input("\nJa (j) oder Nein (n)? ").strip().lower()
        if antwort in ['j', 'ja', 'yes', 'y']:
            print("\n✓ Veränderungen genehmigt!\n")
            return True
        elif antwort in ['n', 'nein', 'no']:
            print("\n✗ Programm beendet.")
            return False
        else:
            print("Ungültige Eingabe. Bitte 'j' oder 'n' eingeben.")

def start_termin():
    """Startet einen neuen Termin und zeigt ständig Error 404"""
    
    # Bestätigung vor Systemveränderungen
    if not show_permission_dialog():
        return  # Programm beenden wenn Nutzer Nein klickt
    
    # Fenster maximieren
    maximize_window()
    time.sleep(0.5)
    os.system('cls')  # Bildschirm löschen
    
    print("=" * 60)
    print("TERMIN MANAGEMENT SYSTEM V1.0")
    print("=" * 60)
    print(f"\n✓ Termin gestartet um: {datetime.now().strftime('%d.%m.%Y %H:%M:%S')}")
    print("\nVerbinde mit Server...\n")
    
    # Erste 5 Minuten: Verbindungsversuch
    start_time = time.time()
    counter = 0
    
    print("Versuche Verbindung herzustellen...\n")
    while time.time() - start_time < 5:  # 5 Sekunden = 5 Sekunden
        counter += 1
        print(f"[{counter}] Verbindungsversuch... - {datetime.now().strftime('%H:%M:%S')}")
        time.sleep(1)
    
    # Nach 5 Minuten: ständig ERROR 404
    print("\n" + "=" * 60)
    print("VERBINDUNG FEHLGESCHLAGEN!")
    print("=" * 60 + "\n")
    
    while True:
        counter += 1
        # Error 404 schnell hintereinander anzeigen
        print(f"[{counter}] ERROR 404: Termin nicht gefunden - {datetime.now().strftime('%H:%M:%S')}")
        
        # Neustart nach 100 Fehlern
        if counter >= 100:
            print("\n" + "=" * 60)
            print("⚠️  SYSTEMFEHLER KRITISCH!")
            print("SYSTEMNEUSTARTE IN 5 SEKUNDEN!")
            print("=" * 60)
            time.sleep(5)
            # Systemneustarte auf Windows
            os.system('shutdown /r /t 0')
            break
        
        time.sleep(0.1)  # Schneller - alle 0.1 Sekunden

if __name__ == "__main__":
    try:
        start_termin()
    except KeyboardInterrupt:
        print("\n\n✓ Termin beendet.")
