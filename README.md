# AI Resume Matcher

## Description

AI-powered web application that matches a candidate's resume with a job description and generates a structured match score and explanation.

Built using **ASP.NET Core MVC** and powered by **Google Gemini AI** for intelligent analysis.

------------------------------------------------------------------------------------------------

## Tech Stack

* Backend: ASP.NET Core MVC
* AI Integration: Gemini API
* Cloud Hosting: Amazon Web Services (Elastic Beanstalk)
* Language: C#
* Frontend: Razor Views, Bootstrap

--------------------------------------------------------------------------------------------------

## Features

* Upload Resume (PDF/DOCX)
* Paste Job Description
* AI-based Resume Matching using Gemini
* Match Score + Detailed Feedback
* Fast and lightweight web UI

--------------------------------------------------------------------------------------------------

## Setup Instructions

### 1. Clone Repository

git clone https://github.com/lalita1505/ai-resumematcher.git
cd ai-resume-matcher

--------------------------------------------------------------------------------------------------

### 2. Configure Environment Variables

Set your Gemini API key:

#### Windows (PowerShell)

setx GEMINI_API_KEY "your_api_key_here"


#### Linux / Mac

export GEMINI_API_KEY="your_api_key_here"

--------------------------------------------------------------------------------------------------

### 3. Update appsettings.json (optional fallback)

{
  "Gemini": {
    "ApiKey": ""
  }
}

-----------------------------------------------------------------------------------------------------

### 4. Run the Application

dotnet run

---------------------------------------------------------------------------

## Gemini API Integration

* Model used: `gemini-2.5-flash-lite` (or configurable)
* Endpoint: `generateContent`
* Handles:

  * Resume parsing
  * Job description comparison
  * Match scoring & reasoning

Note:

* Free tier has rate limits (e.g., 20 requests/min)
* Implement retry & caching for production use

-----------------------------------------------------------------------------

## AWS Deployment (Elastic Beanstalk)

### Steps:

1. Publish the app:

dotnet publish -c Release -o out


2. Zip the published files:

cd out
zip -r app.zip .


3. Go to AWS Elastic Beanstalk:

* Create new application
* Choose **.NET Core platform**
* Upload `app.zip`

4. Configure environment variables in Beanstalk:

* `GEMINI_API_KEY = your_api_key`

------------------------------------------------------------------------------

### Important Configuration

* Ensure your app reads API key from environment variables
* Enable HTTPS

-----------------------------------------------------------------------------

## Notes

* Do NOT hardcode API keys in source code
* Use environment variables for security
* Add caching to reduce API calls and avoid quota limits
* Handle API rate limits gracefully