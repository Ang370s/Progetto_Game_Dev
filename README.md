# 🗝️ Dungeon Escape

Un action dungeon crawler 2D sviluppato in Unity.

Esplora il dungeon, raccogli gemme, sconfiggi nemici e affronta il Boss finale per ottenere la vittoria.

---

## 🎮 Gameplay

- Esplora il dungeon
- Raccogli gemme e pozioni
- Combatti nemici
- Trova la chiave per sbloccare la porta del Boss
- Sconfiggi il Boss finale
- Ottieni il punteggio migliore possibile

---

## 🕹️ Controlli

**W A S D** -> Movimento
**Click sinistro** -> Attacco / Usa Pozione
**F** -> Entra nella porta
**1 2 3** -> Selezione slot inventario

---

## ❤️ Sistema di gioco

### ✔ Vita
- Sistema a cuori
- Le pozioni curano il giocatore

### ✔ Inventario
- Spada
- Arco
- Pozioni curative: max 5; Drop dopo 3 uccisioni o possibilità di trovarle nelle Chest

### ✔ Drop nelle Chest
- 🟡 Oro = 10 punti  
- 🟢 Smeraldo = 50 punti  
- 🔷 Diamante = 200 punti
- Pozione
- Chiave per aprire la porta del Boss

### ✔ Punteggio finale
Il punteggio è basato su:

- Gemme raccolte (Oro: 10 pt; Smeraldo: 50 pt; Diamente: 200 pt)
- Nemici uccisi: 20 pt
- Boss sconfitto: 2000 pt
- Tempo impiegato: parte da 5000 pt a scalare di 10 pt ogni secondo che passa
- Bonus chiave: 500 pt
- Penalità morte giocatore: -5000 pt

---

## 🧟 Boss Fight

Per accedere al Boss:

1. Trova la chiave
2. Avvicinati alla porta
3. Premi **F** (se non hai la chiave viene segnalato da un messaggio visualizzato a schermo)
4. Sopravvivi allo scontro finale

---

## 🏆 Vittoria

Dopo aver sconfitto il Boss:

- Il timer si ferma
- Il punteggio viene calcolato
- Viene mostrata la schermata di vittoria
- Viene salvato automaticamente il punteggio (nelle Record vengono visualizzati i top 5 punteggi)

---

## 🛠️ Tecnologie utilizzate

- Unity
- C#
- TextMeshPro
- Unity Input System

---

## ▶️ Come eseguire il gioco

1. Scarica la cartella **Build**
2. Apri la cartella
3. Avvia: Dungeon's Gate.exe
