<img align=right src="https://github.com/SDU-Sem2Group18/SemesterProject2/blob/v1.0.0/Project.GUI/Deploy/Project.GUI.128x128.png?raw=true">


<h2> Heat Optimisation Manager</h1>
<h4> Syddansk Universitet Sønderborg</h3>
<h6> Summer Semester 2024, Group 18 - Niklas Braun & Rokas Norbutas</h6>


**Project Goal:** Given Data about Production Units, as well as hourly Heat Demand and Electricity Price, create a Desktop Application to optimise the given Data, both for Cost and Emissions.

### To Run
```bash
git clone https://github.com/SDU-Sem2Group18/SemesterProject2 && cd SemesterProject2/Project.GUI
dotnet run
```

### To Publish

1. Make sure you have PupNet installed: 
```bash
dotnet tool install -g KuiperZone.PupNet
```
###
2. For deploying on Windows, make sure you have [InnoSetup](https://jrsoftware.org/isinfo.php) installed.
##
To deploy on Windows:
```bash
pupnet --kind setup
```
##
To Deploy on Linux:
```bash
pupnet --kind appimage
```
Note: AppImage generation is only supported on Linux.
Deployed Files are saved to:
```
Project.GUI/Deploy/bin/*
```

#
Note: As of v1.0.0, the Sample Source Data is contained in the installation directory. Alternatively, it can be found in the source repo [here](https://github.com/SDU-Sem2Group18/SemesterProject2/tree/v1.0.0/Project/Data).

#

# Source Data Formatting
#### Grid Information

- Make sure the csv headers are correct.
- Null Values are not permitted.
- Keep in mind that only one grid is supported.
- Image Paths may be either absolute, or relative to the location of the csv file. Adding an image is optional.

[Sample Data](https://raw.githubusercontent.com/SDU-Sem2Group18/SemesterProject2/v1.0.0/Project/Data/GridInfo.csv):
```csv
name,image,architecture,size
Lillerød,Images/Heatington.png,Single District Heating Network,1600
```
##
#### Unit Information

- Make sure the csv headers are correct.
- While multiple units are supported, keep in mind that the time complexity of the optimiser is O(n²). Furthermore, the larger the images, if available, are, the slower loading and saving the unit data will be.
- Image Paths may be either absolute, or relative to the location of the csv file. Adding images is optional.
- Null Values are denoted with a dash ( - ).

[Sample Data](https://raw.githubusercontent.com/SDU-Sem2Group18/SemesterProject2/v1.0.0/Project/Data/ProductionUnits.csv):
```csv
name,image,max_heat,prod_cost,max_el,gas_cons,co2_ems
Gas Boiler,Images/GasBoiler.png,5,500,-,1.1,215
Oil Boiler,Images/OilBoiler.png,4,700,-,1.2,265
Gas Motor,Images/GasMotor.png,3.6,1100,2.7,1.9,640
Electric Boiler,Images/ElectricBoiler.png,8,50,-8,-,-
```

##
#### Heat Data
- Make sure the csv headers are correct.
- Null Values are not permitted.
- The Date and Time format is: DD/MM/YYYY hh:mm

[Sample Data](https://raw.githubusercontent.com/SDU-Sem2Group18/SemesterProject2/v1.0.0/Project/Data/summer.csv)
```csv
Time from,Time to,Heat Demand,Electricity Price
08/07/2023 00:00,08/07/2023 01:00,1.79,752.03
08/07/2023 01:00,08/07/2023 02:00,1.85,691.05
08/07/2023 02:00,08/07/2023 03:00,1.76,674.78
...
```

##
# Saving and Loading Projects

- Any Project, be it empty or with all source data provided, can be saved to a Heat Optimisation Project (.hop) File.
- Upon saving, the HOP-file no longer depends on the selected source data, including images. It can be savely moved or deleted.
- Opening an HOP-file opens only what is saved within, and overwrites that part of the current project. This means that, for example, if you have two hop-files, one saving only Unit Information and one saving only Source Data, upon loading both after each other, they will both be included in the same project.
- If you wish to merge multiple HOP-files, be sure to create a new project from the main menu, and open your separate HOP files from there. This will allow you to create a new HOP-file with the content of all others.
    - Alternatively, you can open one of your HOP files from the main menu, and add the other HOP files to this one by opening them afterwards. Make sure that you overwrite the right file upon saving.

#

###### Copyright (C) 2024 Niklas Braun & Rokas Norbutas
###### License: Apache-2.0

###### Disclaimer: This Software has absolutely NO affiliation with Danfoss. The Danfoss brand and logo are owned by Danfoss. The Logo is included in this repo solely because this semester project was created by Danfoss, and the repository is required to be public for evaluation.