# reMarkable Sign
reMarkable Sign is an application developed in-house at Eindhoven University of Technology, Netherlands in order to facilitate the digital signing of documents using the reMarkable tablet/e-reader. It attempts to smoothen the process of uploading and downloading single files to/from the reMarkable.

## Why reMarkable Sign?
The nature of the reMarkable tablet makes it very suitable for signing documents or quickly adding handwritten remarks to an existing .pdf file. There are already various options to do these things:
* Using the reMarkable's cloud service.
* Using the reMarkable's web GUI on http://10.11.99.1.
* Using an external file browser.

Using the cloud may not be desired or even legal because of privacy and/or security regulations. The web GUI is a hassle to use and shows all files on the reMarkable, which makes it prone to human error. External file browsers are often overpowered for simple usage which makes them cumbersome to use. To provide a local, fast and secure way to move files to and from the tablet, reMarkable Sign was developed.

## Features
* Fast and easy-to-use uploading and downloading of single files, including drag-and-drop to and from the application.
* Minimal GUI for easy use.
* Completely offline, no internet connection needed on the reMarkable or PC (you will need a USB-C cable, of course).
* Privacy proof: no files are stored or shared anywhere else besides the PC and tablet. It's possible to automatically remove signed documents from the tablet after signing them is complete.
* Secure: since the reMarkable can be disconnected from the internet, it's easy to keep your documents safe.

![Screenshot 3](reMarkableSign_screenshot3.png)

## Installation
Download the .zip file containing the [latest release](https://github.com/fer3852/RemarkableSign/releases). Extract the file to any desired directory. You can run reMarkableSign.exe straight from the extracted folder.

## Usage
For instructions on how to use reMarkable Sign, please refer to [the manual](reMarkableSign_Manual.pdf) (also included with the download).

## License
This project is released under the [MIT License](LICENSE). You're free to use it both privately and commercially.
To make preview images of downloaded files, the [PDFium viewer](https://github.com/pvginkel/PdfiumViewer/blob/master/LICENSE) is used. See its own page for its license.
