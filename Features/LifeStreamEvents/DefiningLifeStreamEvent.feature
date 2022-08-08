Feature: Defining life stream event

    In order to registering and tracking life strams online
    As me
    I want to configure life stream events

Scenario Outline: Life stream event gets defined with its valid properties
When I define a new life stream event with follwoing properties
| Title   | Metadata |
| <title> |          |
Then I can find life stream event '<title>' with above properties

Examples: 
| title                                                                                                                            |
| Init                                                                                                                             |
| Feeling                                                                                                                          |
| Guess                                                                                                                            |
| Assumption                                                                                                                       |
| Conversation                                                                                                                     |
| Conclusion                                                                                                                       |
| ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss |  


Scenario Outline: Life stream event gets defined with matadata and valid properties
Given There are some provided life stream event metadata with following properties
| Title            |
| <metadataTitle1> |
| <metadataTitle2> |
When I define a new life stream event with follwoing properties
| Title   | Metadata |
| <title> |          |
Then I can find life stream event '<title>' with above properties

Examples: 
| title   | metadataTitle1                                                                                                                   | metadataTitle2 |
| Init    | Init Date                                                                                                                        | X              |
| Feeling | ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss | X              |


Scenario: Life stream event gets defined with its unique properties
Given There is a defined life stream event with following properties
 | Title | Metadata |
 | Init  |          |
 When I define a new life stream event with follwoing properties
| Title | Metadata |
| Init  |          |
Then I get error with code 'NEO-LSE-BR-10001' and message 'There is already a registered life stream event with title "Init"' within the system  

Scenario Outline: Life stream event is not allowed to get defined with invalid properties
Given There are some provided life stream event metadata with following properties
| Title            |
| <metadataTitle1> |
| <metadataTitle2> |
When I define a new life stream event with follwoing properties
| Title   | Metadata |
| <title> |          |
Then I get error with code '<errorCode>' and message '<errorMessage>' within the system'  

Examples: 
| title                                                                                                                             | metadataTitle1                                                                                                                     | metadataTitle2 | errorCode        | errorMessage                                                       |
| $Init                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10002 | Life stream event title can not include special character          |
| Init                                                                                                                              | Init%Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Life stream event metadata title can not include special character |
| sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss | Init Date                                                                                                                          | X              | NEO-LSE-BR-10004 | Life stream event title length can not be greater than 128         |
| Feeling                                                                                                                           | ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss s | X              | NEO-LSE-BR-10005 | Life stream event metadata title length can not be greater than 128|

Scenario Outline: Life stream event is not allowed to get modified with invalid properties
Given There is a defined life stream event with following properties
| Title | Metadata |
| Init  |          |
And I have reprovided some life stream event metadata with following properties
| Title            |
| <metadataTitle1> |
| <metadataTitle2> |
When I modify life stream event 'Init' with follwoing properties
| Title   | Metadata |
| <title> |          |
Then I get error with code '<errorCode>' and message '<errorMessage>' within the system'  

Examples: 
| title                                                                                                                             | metadataTitle1                                                                                                                     | metadataTitle2 | errorCode        | errorMessage                                                        |
|                                                                                                                                   | Init Date                                                                                                                          |                | NEO-LSE-BR-10003 | Life stream event title is required                                 |
| $Init                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Life stream event title can not include special characters          |
| &Init                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Life stream event title can not include special characters          |
| #Init                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Life stream event title can not include special characters          |
| !Init                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Life stream event title can not include special characters          |
| [Init                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Life stream event title can not include special characters          |
| Init]                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Life stream event title can not include special characters          |
| (Init                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Life stream event title can not include special characters          |
| Init)                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Life stream event title can not include special characters          |
| Init                                                                                                                              | $Init Date                                                                                                                         | X              | NEO-LSE-BR-10004 | Life stream event metadata title can not include special characters |
| Init                                                                                                                              | Init%Date                                                                                                                          | X              | NEO-LSE-BR-10004 | Life stream event metadata title can not include special characters |
| Init                                                                                                                              | #Init Date                                                                                                                         | X              | NEO-LSE-BR-10004 | Life stream event metadata title can not include special characters |
| Init                                                                                                                              | !Init Date                                                                                                                         | X              | NEO-LSE-BR-10004 | Life stream event metadata title can not include special characters |
| Init                                                                                                                              | [Init Date                                                                                                                         | X              | NEO-LSE-BR-10004 | Life stream event metadata title can not include special characters |
| Init                                                                                                                              | Init Date]                                                                                                                         | X              | NEO-LSE-BR-10004 | Life stream event metadata title can not include special characters |
| Init                                                                                                                              | (Init Date                                                                                                                         | X              | NEO-LSE-BR-10004 | Life stream event metadata title can not include special characters |
| Init                                                                                                                              | Init Date)                                                                                                                         | X              | NEO-LSE-BR-10004 | Life stream event metadata title can not include special characters |
| sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss | Init Date                                                                                                                          | X              | NEO-LSE-BR-10005 | Life stream event title length can not be greater than 128          |
| Feeling                                                                                                                           | ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss s | X              | NEO-LSE-BR-10006 | Life stream event metadata title length can not be greater than 128 |

Scenario Outline: Life stream event is not allowed to get defined with duplicated matadata 
Given There are some provided life stream event metadata with following properties
| Title    |
| Duration |
| Duration |
When I define a new life stream event with follwoing properties
| Title        | Metadata |
| Conversation |          |
Then I get error with code 'NEO-LSE-BR-10007' and message 'There are duplicated metadata' within the system  