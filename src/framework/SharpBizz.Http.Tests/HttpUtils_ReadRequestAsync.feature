@httpSerialization
Feature: HttpUtils_ReadRequestAsync
	Create a HttpRequestMessage from a binary serialized Http request message

Scenario Outline: Read simple requests
	Given A simple HTTP request is recieved with "<method>", "<url>" and HTTP version <major> and <minor>
	When The message is parsed as a HttpRequestMessage
	Then The request method is not null
	And The request method is "<method>"
	And The request url is "<url>"
	And The request version is <major> and <minor>
	And The request headers count is 0
	And The request content is null
	Examples: 
	| method  | url                               | major | minor |
	| GET     | http://www.example.com/           | 1     | 1     |
	| GET     | https://www.example.com/          | 1     | 0     |
	| POST    | http://machine/queuename          | 1     | 1     |
	| OPTIONS | http://authority/path             | 1     | 1     |
	| CUSTOM  | http://authority/?q=hello&a=world | 1     | 1     |

Scenario: Read a GET with simple headers and no content
	Given A GET request is recieved with some standard headers
	When The message is parsed as a HttpRequestMessage
	Then The request method is not null
	And The request method is "GET"
	And The request url is "/hello.txt"
	And The request version is 1 and 1
	And The request headers count is 3
	And The request content is null
