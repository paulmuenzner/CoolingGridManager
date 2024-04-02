<a name="readme-top"></a>


<!-- PROJECT SHIELDS -->
<!--
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
<!-- [![Golang][golang-shield]][golang-url] -->
[![Go Report Card](https://goreportcard.com/badge/github.com/paulmuenzner/coolinggridmanager)](https://goreportcard.com/report/github.com/paulmuenzner/coolinggridmanager)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/b16cd474a2ff4655b973991746193d9c)](https://app.codacy.com/gh/paulmuenzner/coolinggridmanager/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![Issues][issues-shield]][issues-url]
![GitHub go.mod Go version](https://img.shields.io/github/go-mod/go-version/paulmuenzner/coolinggridmanager)
[![GNU License][license-shield]][license-url]
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/paulmuenzner/coolinggridmanager)
![GitHub top language](https://img.shields.io/github/languages/top/paulmuenzner/coolinggridmanager)
 <!-- [![paulmuenzner.com][website-shield]][website-url] -->
[![paulmuenzner github][github-shield]][github-url] 
[![Contributors][contributors-shield]][contributors-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/paulmuenzner/coolinggridmanager">
    <img src="assets/golang-manage-renewable-power-plants.png" alt="Logo" width="342" height="210">
  </a>

  <h3 align="center">C# Cooling Grid Manager</h3>

  <p align="center">
    Billing - Ticket System - Consumption - Efficiency
    <br />
    <a href="#about-the-project"><strong>EXPLORE DOCS</strong></a>
    <br />
    <br />
    <a href="#configuration">High Flexibility</a>
    ·
    <a href="https://github.com/paulmuenzner/coolinggridmanager/issues">Report Bug</a>
    ·
    <a href="https://github.com/paulmuenzner/coolinggridmanager/issues">Request Feature</a>
  </p>
</div>

<!-- HEADER -->
![Header](assets/golang-power-plant-management-server-paul-muenzner.png)

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about-the-project">About The Project</a></li>
    <li><a href="#getting-started">Getting Started</a></li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

The Cooling Grid Manager is a comprehensive software solution designed to efficiently manage and optimize cooling grid systems. This project leverages the power of C# programming language and .NET technologies to deliver a robust and user-friendly application tailored for the needs of cooling grid operators and managers.


### Features
-   ***Ticket Management:*** Streamline ticket creation, tracking, and resolution processes for maintenance, repairs, billing, and other operational tasks.
-   ***Integration:*** Seamlessly integrate with existing data sources, IoT devices, and monitoring systems to centralize data management and enhance operational efficiency.
-   ***Loss Logging:*** Track and log losses within the cooling grid system, facilitating loss analysis, root cause identification, and remedial action implementation.
-   ***Billing:*** Efficiently manage billing processes for cooling grid services, including invoicing, payment tracking, and revenue management.
-   ***Consumer Management:*** Centralize consumer data and interactions, enabling effective communication, service provisioning, and account management.
-   ***Consumption Tracking:*** Monitor and analyze cooling grid consumption patterns to optimize resource allocation, identify inefficiencies, and implement cost-saving measures.



### Technologies Used

-   ***C#:*** The primary programming language used for developing the Cooling Grid Manager, providing a powerful and versatile framework for application development.
-   ***ASP.NET Core:*** Utilized for building scalable and high-performance web applications, enabling cross-platform compatibility and modern web development practices.
-   ***Entity Framework Core:*** Facilitates object-relational mapping (ORM) for database interaction, simplifying data access and management.
-   ***FluentValidation:*** Integrated for comprehensive validation of input data, ensuring data integrity and application reliability.
-   ***Serilog:*** Employed for structured logging, enabling efficient monitoring and troubleshooting of application behavior and errors.

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- GETTING STARTED -->
## Getting Started

Prior to launching the program, clone the repo, install go dependencies and ensure that all configurations are set. 


### Prerequisites 
-   Make sure MongoDB is installed and available.
-   Make sure a properly configured [AWS S3 Bucket](https://aws.amazon.com/s3/?nc1=h_ls) is ready.


### Installation

-   Clone the repo
   ```sh
   git clone https://github.com/paulmuenzner/coolinggridmanager.git
   ```
-   Install go dependencies by running
   ```sh
   go get
   ```

### Environment file (.env)
Before running the program, you need to set up the required environment variables by creating a .env file in the root directory of the project. This file holds sensitive information and configurations needed for the proper functioning of the application.

#### Mandatory Environment Variables

AWS S3 & MongoDB Configuration:

If your application involves interactions with AWS S3, you must provide the following key-value pairs in the .env file:

-   AWS_S3_BUCKET_NAME: The name of your AWS S3 bucket.
-   AWS_REGION: The AWS region where your S3 bucket is located.
-   AWS_ACCESS_KEY_ID: Your AWS access key ID.
-   AWS_SECRET_ACCESS_KEY: Your AWS secret access key.
-   MONGODB_SCHEME: MongoDB Scheme (likely mongodb) 
-   MONGODB_HOST: MongoDB Host (localhost if self-hosted running locally. Read more on [mongodb.com](https://www.mongodb.com/docs/manual/reference/connection-string/).) 
-   MONGODB_PORT: MongoDB Port, eg. 27018 or standard port 27017. Read more on [mongodb.com](https://www.mongodb.com/docs/manual/reference/connection-string/).
-   MONGODB_DATABASE_NAME: Name of your MongoDB database you like to backup.


### Run program

Run program by: `go run main.go` or use live-reloader such as [air](https://github.com/cosmtrek/air) with `air`


<p align="right">(<a href="#readme-top">back to top</a>)</p>




<!-- USAGE -->
## Usage

This server exposes various endpoints to facilitate authentication, file management, and plant-related operations.


### Routes 

#### Authentication 

The authentication API '/auth' offers secure endpoints for user authentication. Response format is always JSON besides file upload which is form-data.

1. **`/auth/registration`**
   - **Method:** POST
   - **Description:** Register a new user with the system.
   - **Authentication Required:** No
   - **Request Body Example:**
     ```json
     {
       "email": "myname@emailexample.com"
     }
     ```

2. **`/auth/verify`**
   - **Method:** POST
   - **Description:** Verify registered account with link using verify token sent to the registered email address.
   - **Authentication Required:** No
   - **Request Body Example:**
     ```json
     {
       "password": "StrongPassword",
       "passwordVerify": "StrongPassword",
       "verifyToken": "y6X2NpA8bPAAdLZRxUT7aqNL-e9eqDcNuIG0VosGg96G0vr4jAkpwGLkDeI0fZwlsfkA65PreOiuZpza4M66cWrz7QYKicjfKWdUUdOAe-v0_EYiA-3D19pK3CS-Vj1U"
     }
     ```

3. **`/auth/signin`**
   - **Method:** POST
   - **Description:** Sign in with valid credentials.
   - **Authentication Required:** No
   - **Request Body Example:**
     ```json
     {
       "email": "myname@emailexample.com",
       "password": "StrongPassword"
     }
     ```

3. **`/auth/signout`**
   - **Method:** POST
   - **Description:** Log out the currently authenticated user.
   - **Authentication Required:** Yes
   - **Request Body Example:**
     ```json
     {}
     ```


#### Tickets

The files API '/files' provides functionality for managing files and documents.

1. **`/api/tickets/addticket`**
   - **Method:** POST
   - **Validation:** Yes
   - **Description:** For the purpose of Incident Management support tickets can be submitted and managed to keep an eye on and solve adverse events.
   <!-- - **Authentication Required:** Yes -->
    - **Request Body Example:**
     ```json
     {
        "title": "Stucking valve",
        "description": "Example description with at least 50 chars for the ticket submitted by me.",
        "category": "technical",
        "priority": "high",
        "reportedBy": "Jon Doe",
        "responsible": "Service worker A",
        "status": "open",
        "statusHistory": [
            {
                "status": "Open",
                "changedDate": "2024-03-25T10:00:00Z"
            }
         ]
     }
     ```


#### Consumption 

API '/consumptions' offers endpoints for managing consumption data of consumers.

1. **`/consumptions/addconsumption`**
   - **Method:** POST
   - **Validation:** Yes
   - **Description:** Aggregate and accumulate cooling energy consumption per user and day (kWh/user*day). The 'LogDate' denotes the timestamp when the value was recorded, while 'ConsumptionDate' indicates the specific day when this consumption occurred. Consumption date cannot be in the future.
   - **Request Body Example:**
     ```json
     {
        "ConsumerID": 123,
        "ConsumptionValue": 45.67,
        "ConsumptionDate": "2024-03-25T10:00:00Z"
     }
     ```

2. **`/plants/log/{apiID:[0-9]+}`**
   - **Method:** POST
   - **Description:** API logging power plant details for a specific plant by providing its ID. Only a numerical ID is accepted.
   - **Authentication Required:** No. However, valid key, secret and apiID are requiered. Furthermore requesting IP must be whitelisted.
   - **Request Body Example:**
     ```json
     {
        "key": "7446579140876818687525890004949221730587",
        "secret": "c2a1d375159502956e552e0e5d57de6735ec",
        "voltageOutput": 40,
        "currentOutput": 2.87,
        "powerOutput": 114.8,
        "solarRadiation": 246,
        "tAmbient": 5,
        "tModule": 5,
        "relHumidity": 77,
        "windSpeed": 5
     }
     ```

#### Consumer 

API '/consumers' offers endpoints for managing consumers.

1. **`/consumers/addconsumer`**
   - **Method:** POST
   - **Validation:** Yes
   - **Description:** Add consumers requesting cooling energy from the cooling grid. Provide ID of existing grid section.
   - **Request Body Example:**
     ```json
     {
        "firstName": "Jon",
        "lastName": "Doe",
        "companyName": "Byte DataCenter Ltd",
        "email": "email@jondoe.com",
        "phone": "123456789",
        "gridSectionID": 17
    }
     ```


Feel free to explore and integrate these API routes into your applications! If you have any questions or need further assistance, please refer to the detailed documentation for each route.

### Statistical Analysis

Possible calculations:

-   Interquartile Range
-   Lower bound
-   Mean
-   Median
-   Outliers
-   Quantile 25, Quantile 75, Quantile 90, Quantile 95
-   Skewness
-   Standard deviation
-   Upper bound
-   Variance


<!-- ROADMAP -->
## Roadmap

-   ✅ Storing images on and implementing AWS S3 
-   ✅ Extend email notification feature
-   ⬜️ Addressing more nuanced linting issues.
-   ⬜️ Implement brute-force protection for authorization process
-   ⬜️ Extend testing
-   ⬜️ Add option to backup and upload MongoDB database to S3


See the [open issues](https://github.com/paulmuenzner/coolinggridmanager/issues) to report bugs or request fatures.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

Contributions to the Cooling Grid Manager project are welcome! Whether you're interested in adding new features, fixing bugs, or improving documentation, your contributions are highly valued. To get started, fork the repository, make your changes, and submit a pull request. 

See [CONTRIBUTING.md](CONTRIBUTING.md) for more info.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the GNU General Public License v2.0. See [LICENSE](LICENSE.txt) for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Paul Münzner: [https://paulmuenzner.com](https://paulmuenzner.com) 

Project Link: [https://github.com/paulmuenzner/coolinggridmanager](https://github.com/paulmuenzner/coolinggridmanager)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

Use this space to list resources you find helpful and would like to give credit to. I've included a few of my favorites to kick things off!

*   [AWS S3 Upload Size](https://docs.aws.amazon.com/AmazonS3/latest/userguide/upload-objects.html)
*   [MongoDB Go Docs](https://www.mongodb.com/docs/drivers/go/current/quick-start/)
*   [AWS SDK for Go V2 Docs][aws-url]
*   [Gomail Docs](https://pkg.go.dev/gopkg.in/gomail.v2?utm_source=godoc)
*   [Testing](https://pkg.go.dev/testing) & [assert](https://pkg.go.dev/github.com/stretchr/testify/assert)


<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[golang-shield]: https://img.shields.io/badge/golang-black.svg?logo=go&logoColor=ffffff&colorB=00ADD8
[golang-url]: https://go.dev/
[aws-shield]: https://img.shields.io/badge/aws_s3-black.svg?logo=amazons3&logoColor=ffffff&colorB=569A31
[aws-url]: https://aws.github.io/aws-sdk-go-v2/docs/
[mongodb-shield]: https://img.shields.io/badge/mongodb-black.svg?logo=mongodb&logoColor=ffffff&colorB=47A248
[mongodb-url]: https://go.dev/
[github-shield]: https://img.shields.io/badge/paulmuenzner-black.svg?logo=github&logoColor=ffffff&colorB=000000
[github-url]: https://github.com/paulmuenzner?tab=repositories
[contributors-shield]: https://img.shields.io/github/contributors/paulmuenzner/coolinggridmanager.svg
[contributors-url]: https://github.com/paulmuenzner/coolinggridmanager/graphs/contributors
[issues-shield]: https://img.shields.io/github/issues/paulmuenzner/coolinggridmanager.svg
[issues-url]: https://github.com/paulmuenzner/coolinggridmanager/issues
[license-shield]: https://img.shields.io/badge/license-GPL_2.0-orange.svg?colorB=FF5733
[license-url]: https://github.com/paulmuenzner/coolinggridmanager/blob/master/LICENSE.txt
<!-- [website-shield]: https://img.shields.io/badge/www-paulmuenzner.com-blue
[website-url]: https://paulmuenzner.com -->

