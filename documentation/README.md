
# Documentation

## **Adding Custom Button to show Frequently Updated Items on Sitecore Admin Panel**


## Summary

**Category:** Best enhancement to the Sitecore Admin (XP) UI for Content Editors & Marketers.

 - **Purpose**: To enhance the content editor accessibility by providing a list of frequently updated items on Sitecore Admin Panel.
 - **Problem Solved**:  There is no feature available to show recently updated items path in a Sitecore admin panel, Content Editor has to traverse the complete Sitecore content tree to update an item frequently.
 - **How it works**: In the solution provided, a custom button named as *'Frequently Visited'* is provided on the Home Tab, which displays a top 10 clickable list of recently updated Sitecore Item path on the grid panel. On click of the listed item path, it opens the respective item in the content tree.

## Pre-requisites

 - Sitecore 9

## Installation

 Use the Sitecore Installation wizard to install the  [packages](https://github.com/Sitecore-Hackathon/2019-Dynamic-Developers/tree/master/sc.package) listed below:

 1. DynamicDeveloper_CoreItemLibrary.zip
 2. DynamicDevelopers_FileLibrary.zip


## Usage

 1. Open content editor and update sitecore items and save it.	
		 ![Content Editor](images/dd_contenteditor.png?raw=true "Content Editor")
 2. Navigate to Home tab.
    ![Content Editor](images/dd_contenteditor_home.png?raw=true "Content Editor")
 3. Click on *'Frequently Visited'* custom button, it displays the top 10 list of recently updated items path in a grid panel.
  ![Content Editor](images/dd_contenteditor_list.png?raw=true "Content Editor")
 4. On a click of the item, it opens the respective item in the content tree.
![Content Editor](images/dd_contenteditor_list_selected?raw=true "Content Editor")
![Content Editor](images/dd_contenteditor_list_final?raw=true "Content Editor")

## Video

The link to the video can be found [here](https://youtu.be/3bBunB1_i8Y). The items used for the demo can be found [here](https://github.com/Sitecore-Hackathon/2019-sitecore-crusaders/blob/master/sc.package/Data_Export_Import_Demo_Package-v1.zip?raw=true)