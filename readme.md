# AzDoBridge

This is an Open-Source initiative pointing out a way to use Amazon Alexa, Google Home or Bot Frameworks along with Azure Functions to have a sort of Voice-Access command to Azure DevOps.

## Requests

By Calling the skill Name e.g. "Azure DevOps", a luanch request to Azure DevOps Organization is invoked; then waiting for a request statement.

possible requests that are implemented so far:

- Azure DevOps WorkItem Status change: by saying "set itemnumber as itemstatus"
- WorkItem change owner: by saying "Assign itemnumber to employeename"
- Set priority for a WorkItem : by saying "Set itemnumber priority to itempriority"

the logic keep listening for max of 8 seconds for further requests otherwise session will terminate.

You can provide no more requests by saying "Cancel Azure Devops"

## What's next:

There is a huge possiblity to take advantage of Azure DevOps Client by implementing more complex scenarios like providing statistics or automate changes.

Stay tuned!

## License 

> MIT License
> 
> Copyright (c) 2019 AzDoBridge
> 
> Permission is hereby granted, free of charge, to any person obtaining a copy
> of this software and associated documentation files (the "Software"), to deal
> in the Software without restriction, including without limitation the rights
> to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
> copies of the Software, and to permit persons to whom the Software is
> furnished to do so, subject to the following conditions:
> 
> The above copyright notice and this permission notice shall be included in all
> copies or substantial portions of the Software.
> 
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
> IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
> FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
> AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
> LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
> OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
> SOFTWARE.

See [License](LICENSE)

## News

* [ALEXA, OPEN AZURE DEVOPS](https://writeabout.net/2019/01/10/alexa-open-azure-devops/)

## Build Status
[![Build Status](https://dev.azure.com/devoteam-alegri/AzDevSkill/_apis/build/status/AzDOBridge.azure-devops-bridge?branchName=master)](https://dev.azure.com/devoteam-alegri/AzDevSkill/_build/latest?definitionId=2&branchName=master)
