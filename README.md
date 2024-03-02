# ColorApp

![ColorApp banner image](./Editor/Resources/Images/ColorAppIcon.png)

## What is CalorApp
Custom editor tool for Unity 3D that allows you to create and edit color palettes and apply them to graphics in your project

Welcome to the ColorApp Unity Package documentation. This package provides a simple tool for creating and managing color palettes in Unity, allowing you to easily assign colors to various elements in your game or app.

## Getting Started
* The first step is to open the color palette editor, **Window/ColorApp/ColorAppEditor** in this panel you can manage the color palettes.

## Creating color palettes:
To create color palettes you must assign a configuration file, you can create one or use the default file.
* Create a list of color labels.
* Add a palette to the palette list and give it a name.

## Managing Color Palettes
* To modify the color palettes you can load the data from the configuration file by pressing the LoadPalette button.
* To save the created data to the configuration file, press the Save Palettes button.

## Assigning Color Palettes
* Add an element to the scene and assign the **ColorizerHandler** script and select the color palette you want to apply.
* To assign color palettes you must assign one of the Colorizer type scripts to a Canvas, Sprite renderer or modeling element and choose the color label you want to apply.
* Again in the **ColorizerHandler** script Press the **Colorizer All** button or you can call this function from some external code at runtime

## Support and Contact
For further assistance or inquiries, please contact our support team at jairoandrety@hotmail.com