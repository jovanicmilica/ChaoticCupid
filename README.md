# ChaoticCupid

ChaoticCupid is a WCF-based PubSub application that simulates a chaotic cupid matchmaking service. The system consists of two interfaces – one for persons and one for the cupid – built using duplex WCF communication over NetTcpBinding.

## Features

- Person registration with input validation
- Automatic letter sending every minute based on a scoring algorithm (location, age, random factor)
- Blocking specific users from sending letters
- One letter at a time per person – confirmation required before receiving the next

## Project Structure

- ChaoticCupid.Service – core logic, contracts and models
- ChaoticCupid.Host – console app that hosts the WCF service
- ChaoticCupid.Client – console app for person registration and letter receiving

## Tech Stack

- .NET Framework 4.7.2
- WCF with NetTcpBinding (duplex communication)
- RNGCryptoServiceProvider for random scoring

## How to Run

- Start ChaoticCupid.Host
- Start one or more instances of ChaoticCupid.Client
- Register with username, city, age and phone
- Wait for letters – type /block username to block someone, press Enter to confirm a received letter
