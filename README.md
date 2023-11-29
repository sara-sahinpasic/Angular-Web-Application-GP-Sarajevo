<h3>Angular - Web Application: "ePrijevozSarajevo"</h3>
<i>Seminar work - Software development 1 - Faculty of Information Technologies</i> </br>
<i>Team: Sara Šahinpašić & Amor Osmić</i> </br>

## Description
The Angular web application for online ticket sales for public transport in Sarajevo is a project that allows users to purchase transport tickets online. Users will have the opportunity to purchase all types of transport tickets. In addition to online ticket sales, the application will offer the possibility of reporting vehicle malfunctions and potential delays.
Users will be able to get acquainted with all destinations covered by public city transport in Sarajevo, the timetable and departure stations of the selected mode of transport. All this greatly contributes to better time planning of the day in Sarajevo.

## 🤝 Collaboration

This Angular web application was developed in close collaboration with my colleague **[Amor Osmić](https://github.com/Anersyum)** as part of a two-person development team. The project followed the **Scrum methodology**, with a total of **10 sprints**, planned and tracked using **Azure DevOps (DevOps Boards)** for sprint planning, task assignments, daily stand-ups, and sprint reviews.

We applied solid software engineering principles and practices throughout the project, including:

* Backend built with **.NET 6.0**
* **Authentication & Authorization** using **Identity Server 4**, **JWT tokens**, and **two-factor authentication**
* Features such as **email notifications**, **file uploads**, and **centralized exception handling**
* A **multilingual Angular frontend** featuring:

  * **SVG graphics**
  * **Image upload and display**
  * **Toast messages (JavaScript notifications)**
* Clean architecture on the backend with the **Repository** and **Unit of Work patterns**
* **Paging with server-side processing**, **parameterized reports**, and **view models** for clear data presentation

In addition to development, we documented the project thoroughly with:

* A detailed **vision document** describing project goals and scope
* **Use case** and **domain model diagrams** to guide system design
* Interactive **mockups** created and refined for each sprint (10 in total)

We used **Git** for version control, regularly conducted **code reviews**, and worked closely together through pair programming and team meetings to ensure consistent quality and collaboration.

## Running the code
- Clone the project `git clone https://github.com/sara-sahinpasic/Angular-Web-Application-GP-Sarajevo.git`
- Navigate to repository root

### Run backend
- Navigate to ´RestAPI\Prodaja karata za gradski prijevoz´, open and then start ´Prodaja karata za gradski prijevoz.sln´

### Run Web App  (Frontend)
To start the Angular frontend of the application:
- Navigate to the folder: `SPA/src/app`.
- Open **Visual Studio Code**.
- Open the folder in VS Code using the terminal command: ```code .``` or
- Open it directly through the VS Code interface.
- In the terminal, run the Angular development server: ```ng serve```
- Open your browser and go to: http://localhost:4200/

## Credentials

### Web App (Administrator):
	E-mail: sara.sahinpasic@hotmail.com
	Password: 0000

### Web App (Driver):
	E-mail: driver@mail.com
	Password: 0000
    
### Web App (User):
To log in as a user on the web application:
- Open SQL Server Management Studio.
- In the Databases folder, locate the generated database: GPSarajevo.
- Open the Users table and choose one of the seeded email addresses.
- Use the following credentials to log in:
### 
	E-mail: (Use one of the emails from dbo.Users)
	Password: 0000


## 📸 Application Overview (Screenshots)

_These are just a few selected screenshots to help you quickly understand the core features and look of the application._

## 🛠️ Admin
<p float="left">
  <img src="Screenshots/Admin/1. login.png" width="850" />
  <img src="Screenshots/Admin/2. section_start.png" width="850" />
  <img src="Screenshots/Admin/3. section_users.png" width="850" />
  <img src="Screenshots/Admin/5. section_edit_users_data.png" width="850" />
  <img src="Screenshots/Admin/6. section_company.png" width="850" />
  <img src="Screenshots/Admin/7. section_add_company_new_info_1.png" width="850" />
  <img src="Screenshots/Admin/8. section_add_company_new_info_2.png" width="850" />
  <img src="Screenshots/Admin/9. section_statistic.png" width="850" />
</p>

## 👤 User
<p float="left">
<img src="Screenshots/User/1. search_routes.png" width="850" />
<img src="Screenshots/User/2. user_info.png" width="850" />
<img src="Screenshots/User/3. user_request.png" width="850" />
<img src="Screenshots/User/4. user_purchase_history.png" width="850" />
<img src="Screenshots/User/5. review_section_1.png" width="850" />
<img src="Screenshots/User/6. review_sction_2.png" width="850" />
<img src="Screenshots/User/7. news_section_1.png" width="850" />
<img src="Screenshots/User/8. news_section_2.png" width="850" />
</p>

## 🚌 Driver
<p float="left">
  <img src="Screenshots/Driver/1. section_delay.png" width="850" />
  <img src="Screenshots/Driver/2. section_manufacturer.png" width="850" />
</p>

