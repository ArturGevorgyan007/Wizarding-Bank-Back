# Wizarding-Bank-Back

Center of Excellence
3.28.2023
Table of Contents
Project Requirements
Summary
Core Functional Scope
Standard Functional Scope
Non-functional Scope
Definition of Done
Project Expectations
Security Standards
Nice-to-Have's

## Requirements

The Wizarding Bank project aims to develop a secure, user-friendly, and versatile financial
application for both personal and business users. The core functionalities for personal
accounts include sending or requesting money, linking bank accounts, and managing
transaction history, while business accounts offer additional features such as applying
for loans and accepting customer payments. The project's completion will be
demonstrated through a cloud-hosted working version, technical presentation, and
associated diagrams.

## Core Functional Scope

### Personal Account

As a person, I should be able to:
Send or request money by username, email, or phone number.
Add credit or debit cards to my account using card details
Link a bank account to my account using routing number and account number
Add Cash to Wizard wallet
View my transaction history


### Business Account

As a business, I should be able to:
Add a business credit or debit card using card details
Apply for a business loan (interest rate depends on business type => professor, student, small business, auror, etc)
Accept payments from customers (checkout)
View business transaction history
Export PDF Statement for WizBank transactions

## Standard Functional Scope
Access settings to change any personal or business details
Login as personal or business account
Register business as a for-profit or nonprofit account with email, password, Business Name, Business Address and Business Identification Number (BIN, EIN, etc.)
Register new personal account with email, password, full name, phone number, and primary address
Logout of the application
Reset password through security questions or IDaaS providers (auth0 ?)
Responsive to different, or changing, window sizes
Supported by common browsers (Chrome, Edge, Firefox, Safari)

## Non-Functional Scope
80% unit testing on API business logic (Service layer)
50-60% unit testing on Angular Frontend
Scrum/Agile Development Strategy with Burndown charts
RESTful API implementation
HTTPS support
API logging and exception handling
Comments in the code base with respective details
Development Process documentation (Scrum Master, Team structure, Git Practice, Branch Protection, etc.)
CI/CD pipeline for Backend and Frontend applications using GitHub actions
Sonarcloud integration for backend and frontend
Containerized Backend and deployed to azure as container deployment
Utilize Entity Framework Core to interact with database

## Expectations
Definition of Done
Presentation of technical details of the application
Working version of the cloud hosted application demonstration
Sharing the associates’ code repo for technical evaluation with:
ERD Diagram
Architecture Diagram
Activity Diagram

## Security Standards
Password Hashing implementation
Session management with web tokens (JWT)
CORS API restrictions
Only allow deployed client and localhost to access API

## Nice-to-Have's
Email notifications for monthly statements
Receive email notifications when a transaction is made
Export PDF Statement for RevPay wallet
Wizard-Themed Login (Brick wall tapping?)

### Personal Accounts
Pay with QR code linked to Business UPID or Account
Deals and Cashback options
Generate Reward Points
Buy Now, Pay Later
Pay with Rewards
Pay bills

### Business Accounts
Generate a QR code linked to UPID or Account
Accept donations (nonprofits only)
Accept international with currency conversions

### General:
Customer Support Forum or Chat
Mobile responsive
Accessibility considerations for disabilities
API role-based endpoint authorization
Input validation and error handling
Data encryption for secure API calls
Audit logging
Multi-factor authentication
	
## Additional Requirements: 
Development Process documentation (Scrum Master, Team structure, Git Practice, Branch Protection, etc.)
CI/CD pipeline for Backend and Frontend applications using GitHub actions
Sonarcloud integration for backend and frontend
Containerized Backend and deployed to azure as container deployment
Utilize Entity Framework Core to interact with database
