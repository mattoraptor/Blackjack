# Blackjack

##A refactoring kata

####Project structure:
1. Blackjack - The main program. It does a blackjack.
2. BlackjackTests - Test assembly. Some assembly required.

####Tools:
`runcoverage.bat` will run nunit tests and show a report showing your current code coverage


## First Story - Done
Player can choose wager between 1 and 50, and it uses the wager correctly in the game.
Also the wager is clamped to the allowed range.

## Second Story - In Progress
Wagers are paid out 3:2 (wagering $100 pays $150 on a win). Losing costs what you wager.