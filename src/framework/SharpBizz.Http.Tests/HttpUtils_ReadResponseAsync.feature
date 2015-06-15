@httpSerialization
Feature: HttpUtils_ReadResponseAsync
	Create a HttpResponseMessage from a binary serialized Http response message

Scenario Outline: Read a simple response message
	Given A simple HTTP response is recieved with "<status>", "<reason>" and HTTP version 1 and 1
	When  The message is parsed as a HttpResponseMessage
	Then  The response is not null
	And   The response status code is "<status>"
	And   The response reason phrase is "<reason>"
	Examples:
	| status | reason                |
	| 100    | Continue              |
	| 200    | OK                    |
	| 201    | Created               |
	| 301    | Moved Permanently     |
	| 404    | Not Found             |
	| 500    | Internal Server Error |

Scenario Outline: Read a simple response message with different HTTP versions
	Given A simple HTTP response is recieved with "200", "OK" and HTTP version <major> and <minor>
	When  The message is parsed as a HttpResponseMessage
	Then  The response is not null
	And   The response version is <major> and <minor>
	Examples:
	| major | minor |
	| 1     | 1     |
	| 1     | 0     |
	| 1     | 7     |
	| 2     | 0     |
	| 0     | 9     |
