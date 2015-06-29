@httpSerialization
Feature: HttpUtils_WriteResponseAsync
	Create a binary serialized version of a given HttpResponseMessage

Scenario Outline: Write a simple response message
	Given A simple HTTP response is created with <status>, "<reason>" and HTTP version 1 and 1
	When  The response message is written to a stream
	Then  The stream is not empty
	And   The string representation of the stream is a simple response message with <status>, "<reason>" and HTTP version 1 and 1
	Examples:
	| status | reason                |
	| 100    | Continue              |
	| 200    | OK                    |
	| 201    | Created               |
	| 301    | Moved Permanently     |
	| 404    | Not Found             |
	| 500    | Internal Server Error |

Scenario Outline: Write a simple response message with different HTTP versions
	Given A simple HTTP response is created with 200, "OK" and HTTP version <major> and <minor>
	When  The response message is written to a stream
	Then  The stream is not empty
	And   The string representation of the stream is a simple response message with 200, "OK" and HTTP version <major> and <minor>
	Examples:
	| major | minor |
	| 1     | 1     |
	| 1     | 0     |
	| 1     | 7     |
	| 2     | 0     |
	| 0     | 9     |


Scenario Outline: Write a simple response message with one content header and no content
	Given A simple response is created with content "<header>" with value "<value>" and empty string content
	When  The response message is written to a stream
	Then  The stream is not empty
	And The string representation of the stream is a simple response with "<header>" with value "<value>" and plain text content type
	Examples:
	| header              | value                            |
	| Content-Disposition | attachment; filename="fname.ext" |
	| Content-Location    | /index.htm                       |
	| Content-MD5         | Q2hlY2sgSW50ZWdyaXR5IQ==         |
	| Content-Range       | bytes 21010-47021/47022          |
	| Expires             | Thu, 01 Dec 1994 16:00:00 GMT    |
	| Last-Modified       | Tue, 15 Nov 1994 12:45:26 GMT    |
	| Allow               | GET, HEAD, OPTIONS               |
	| Content-Encoding    | gzip, lzha                       |
	| Content-Language    | da, en-US, pt-PT, es-ES          |

Scenario Outline: Write a simple response message with one response header and no content
	Given A simple response is created with response "<header>" with value "<value>"
	When  The response message is written to a stream
	Then  The stream is not empty
	And The string representation of the stream is a simple response with "<header>" with value "<value>"
	Examples:
	| header                      | value                                                                                                                      |
	| Access-Control-Allow-Origin | *                                                                                                                          |
	| Accept-Patch                | text/example;charset=utf-8                                                                                                 |
	| Accept-Ranges               | bytes                                                                                                                      |
	| Age                         | 12                                                                                                                         |
	| Connection                  | close                                                                                                                      |
	| Cache-Control               | max-age=3600                                                                                                               |
	| Date                        | Tue, 15 Nov 1994 08:12:31 GMT                                                                                              |
	| ETag                        | "737060cd8c284d8af7ad3082f209582d"                                                                                         |
	| Link                        | </feed>; rel="alternate"                                                                                                   |
	| Location                    | http://www.w3.org/pub/WWW/People.html                                                                                      |
	| P3P                         | CP="This is not a P3P policy! See http://www.google.com/support/accounts/bin/answer.py?hl=en&answer=151657 for more info." |
	| Pragma                      | no-cache                                                                                                                   |
	| Proxy-Authenticate          | Basic                                                                                                                      |
	| Public-Key-Pins             | max-age=2592000; pin-sha256="E9CZ9INDbd+2eRQozYqqbQ2yXLVKB9+xcprMF+44U1g=";                                                |
	| Refresh                     | 5; url=http://www.w3.org/pub/WWW/People.html                                                                               |
	| Retry-After                 | 120                                                                                                                        |
	| Retry-After                 | Fri, 07 Nov 2014 23:59:59 GMT                                                                                              |
	| Set-Cookie                  | UserID=JohnDoe; Max-Age=3600; Version=1                                                                                    |
	| Strict-Transport-Security   | max-age=16070400; includeSubDomains                                                                                        |
	| Trailer                     | Max-Forwards                                                                                                               |
	| Transfer-Encoding           | gzip                                                                                                                       |
	| TE                          | chunked, compress, deflate, gzip, identity                                                                                 |
	| Server                      | Apache/2.4.1 (Unix)                                                                                                        |
