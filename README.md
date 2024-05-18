<a name="readme-top"></a>


<!-- PROJECT SHIELDS -->
[![Issues][issues-shield]][issues-url]
[![GNU License][license-shield]][license-url]
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/paulmuenzner/CoolingGridManager)
![GitHub top language](https://img.shields.io/github/languages/top/paulmuenzner/CoolingGridManager)
[![paulmuenzner github][github-shield]][github-url] 
[![Contributors][contributors-shield]][contributors-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">

  <h3 align="center">C# Cooling Grid Manager</h3>

  <p align="center">
    Billing - Ticket System - Consumption - Efficiency
    <br />
    <a href="#about-the-project"><strong>EXPLORE DOCS</strong></a>
    <br />
    <br />
    <a href="#about-the-project">Plenty of features</a>
    ·
    <a href="https://github.com/paulmuenzner/CoolingGridManager/issues">Report Bug</a>
    ·
    <a href="https://github.com/paulmuenzner/CoolingGridManager/issues">Request Feature</a>
  </p>
</div>


<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about-the-project">About The Project</a></li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

This Cooling Grid Manager is a software solution designed to efficiently manage and optimize cooling grid systems. This project leverages the power of C# programming language and .NET technologies to deliver a robust and user-friendly application tailored for the needs of cooling grid operators and managers. See it as a starting point and develop and extend it as per your needs.


### Features
-   ***Ticket Management:*** Streamline ticket creation, tracking, and resolution processes for maintenance, repairs, billing, and other operational tasks.
-   ***Integration:*** Seamlessly integrate with existing data sources, IoT devices, and monitoring systems to centralize data management and enhance operational efficiency.
-   ***Parameter Logging:*** Track and log grid parameters for each cooling grid system, facilitating loss analysis, root cause identification, and remedial action implementation.
-   ***Billing:*** Efficiently manage billing processes for cooling grid services, including invoicing, payment tracking, and revenue management.
-   ***Consumer Management:*** Centralize consumer data and interactions, enabling effective communication, service provisioning, and account management.
-   ***Consumption Tracking:*** Monitor and analyze cooling grid consumption patterns to optimize resource allocation, identify inefficiencies, and implement cost-saving measures.



### Technologies Used

-   ***C#:*** The primary programming language used for developing the Cooling Grid Manager, providing a powerful and versatile framework for application development.
-   ***ASP.NET Core:*** Utilized for building scalable and high-performance web applications, enabling cross-platform compatibility and modern web development practices.
-   ***Entity Framework Core:*** Facilitates object-relational mapping (ORM) for database interaction, simplifying data access and management.
-   ***FluentValidation:*** Integrated for comprehensive validation of input data, ensuring data integrity and application reliability.
-   ***Serilog:*** Employed for structured logging, enabling efficient monitoring and troubleshooting of application behavior and errors.
-   ***Cron Jobs:*** Implemented for automating tasks such as creating bills and calculating grid efficiency and energy transfer at the start of each month, enhancing efficiency and reducing manual intervention.
-   ***Swagger:*** Integrated Swagger for API documentation, offering a user-friendly interface to explore and interact with API endpoints, facilitating development and collaboration.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- USAGE -->
## Usage

### Run program

Run program by: `dotnet run` 
Build program by: `dotnet build` 


<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Endpoints 

This server exposes various endpoints to facilitate authentication, file management, and plant-related operations.
Please review implemented swagger information for more details at: http://localhost:5286/index.html

#### Tickets

The files API '/tickets' provides functionality for creating and updating (eg. status "solved") support tickets.
Currently available options for status are 'open', 'onhold' and 'solved'.
For the purpose of Incident Management, support tickets can be submitted and managed to keep an eye on and solve adverse events.


#### Consumption 

API '/consumptions' offers endpoints for managing consumption data of consumers.


#### Billing 

API '/billing' offers endpoints for managing consumer bills.


#### Consumer 

API '/consumers' offers endpoints for managing consumers, for example creation of a new consumer or retrieving consumer details connected to a grid section of a grid.


#### Grids 

API '/grids' offers endpoints for managing cooling grid systems, for example creation of a new grid.


#### Grid Sections 

Subdivide each grid into sections with API '/gridsections' for better overview and management.


#### Grid Parameters 

API '/gridparameters' offers endpoints for managing cooling grid systems. 
For example, add new grid parameter log, such as mass flow rate (in kg/s), specific heat capacity of the heating fluid (in J/kg⋅K) as well as temperature inlet and outlet (K) which is needed to calculate the temperature difference ΔT = T_in - T_out. The following formula can be used to calculate the energy flow (cooling energy transfer rate) through the pipe: Energy Flow (Q) = Mass Flow Rate (m) * Specific Heat Capacity (Cp) * Temperature Difference (ΔT). Determine the appropriate frequency for data collection based on your monitoring needs. A higher frequency provides more detailed information but requires more storage and processing resources.


#### Grid Energy Transfer 

API '/GridEnergyTransfer' offers endpoints for managing the transfered energy per month. Based on on the logged parameters of the grid, the entire energy transfer in kWh is calculated for each month based on E_month = (m_dot) * (cp) * (T_in - T_out) * (t) / (3600 * 1000). T_in represents the average flow temperature into the grid; T_out the return flow temperature. t reflects the time in seconds. The difference between energy transfer of the grid and total consumer consumption reflects grid losses.



<!-- ROADMAP -->
## Roadmap

-   ✅ Cron job implementation calculating and storing grid energy transfer
-   ✅ Cron job implementation calculating and storing grid efficiency
-   ⬜️ Implementing Authentication and Authorization 
-   ⬜️ Providing statistical evaluation
-   ⬜️ Extend testing
-   ⬜️ Add email notifications


See the [open issues](https://github.com/paulmuenzner/CoolingGridManager/issues) to report bugs or request fatures.

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

Project Link: [https://github.com/paulmuenzner/CoolingGridManager](https://github.com/paulmuenzner/CoolingGridManager)

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
[contributors-shield]: https://img.shields.io/github/contributors/paulmuenzner/CoolingGridManager.svg
[contributors-url]: https://github.com/paulmuenzner/CoolingGridManager/graphs/contributors
[issues-shield]: https://img.shields.io/github/issues/paulmuenzner/CoolingGridManager.svg
[issues-url]: https://github.com/paulmuenzner/CoolingGridManager/issues
[license-shield]: https://img.shields.io/badge/license-GPL_2.0-orange.svg?colorB=FF5733
[license-url]: https://github.com/paulmuenzner/CoolingGridManager/blob/master/LICENSE.txt
<!-- [website-shield]: https://img.shields.io/badge/www-paulmuenzner.com-blue
[website-url]: https://paulmuenzner.com -->

