Feature: Modifying Stream event type

In order to registering and tracking life strams online
As me
I want to configure Stream event types

    Scenario Outline: Stream event type gets modified with its valid properties
        Given There are some provided stream event type metadata with following properties
          | Title     |
          | Init Date |
        And There is a defined stream event type with following properties
          | Title | Metadata |
          | Init  |          |
        And I have reprovided some stream event type metadata with following properties
          | Title               |
          | <newMetadataTitle1> |
          | <newMetadataTitle2> |
        When I modify stream event type 'Init' with follwoing properties
          | Title      | Metadata |
          | <newTitle> |          |
        Then I can find stream event type '<newTitle>' with above properties

        Examples:
          | newTitle                                                                                                                         | newMetadataTitle1 | newMetadataTitle2 |
          | Conversation                                                                                                                     | Duration          | IsClosed          |
          | ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss | X                 | Z                 |

    Scenario: Stream event type gets modified with its unique properties
        Given There is a defined stream event type with following properties
          | Title | Metadata |
          | Init  |          |
        And There is a defined stream event type with following properties
          | Title        | Metadata |
          | Conversation |          |
        When I modify stream event type 'Init' with follwoing properties
          | Title        | Metadata |
          | Conversation |          |
        Then I get error with code 'NEO-LSE-BR-10001' and message 'There is already a registered Stream event type with title "Conversation"' within the system

    Scenario Outline: Stream event type is not allowed to get modified with invalid properties
        Given There is a defined stream event type with following properties
          | Title | Metadata |
          | Init  |          |
        And I have reprovided some stream event type metadata with following properties
          | Title            |
          | <metadataTitle1> |
          | <metadataTitle2> |
        When I modify stream event type 'Init' with follwoing properties
          | Title   | Metadata |
          | <title> |          |
        Then I get error with code '<errorCode>' and message '<errorMessage>' within the system'

        Examples:
          | title                                                                                                                             | metadataTitle1                                                                                                                     | metadataTitle2 | errorCode        | errorMessage                                                        |
          |                                                                                                                                   | Init Date                                                                                                                          |                | NEO-LSE-BR-10003 | Stream event type title is required                                 |
          | $Init                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Stream event type title can not include special characters          |
          | &Init                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Stream event type title can not include special characters          |
          | #Init                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Stream event type title can not include special characters          |
          | !Init                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Stream event type title can not include special characters          |
          | [Init                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Stream event type title can not include special characters          |
          | Init]                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Stream event type title can not include special characters          |
          | (Init                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Stream event type title can not include special characters          |
          | Init)                                                                                                                             | Init Date                                                                                                                          | X              | NEO-LSE-BR-10003 | Stream event type title can not include special characters          |
          | Init                                                                                                                              | $Init Date                                                                                                                         | X              | NEO-LSE-BR-10004 | Stream event type metadata title can not include special characters |
          | Init                                                                                                                              | Init%Date                                                                                                                          | X              | NEO-LSE-BR-10004 | Stream event type metadata title can not include special characters |
          | Init                                                                                                                              | #Init Date                                                                                                                         | X              | NEO-LSE-BR-10004 | Stream event type metadata title can not include special characters |
          | Init                                                                                                                              | !Init Date                                                                                                                         | X              | NEO-LSE-BR-10004 | Stream event type metadata title can not include special characters |
          | Init                                                                                                                              | [Init Date                                                                                                                         | X              | NEO-LSE-BR-10004 | Stream event type metadata title can not include special characters |
          | Init                                                                                                                              | Init Date]                                                                                                                         | X              | NEO-LSE-BR-10004 | Stream event type metadata title can not include special characters |
          | Init                                                                                                                              | (Init Date                                                                                                                         | X              | NEO-LSE-BR-10004 | Stream event type metadata title can not include special characters |
          | Init                                                                                                                              | Init Date)                                                                                                                         | X              | NEO-LSE-BR-10004 | Stream event type metadata title can not include special characters |
          | sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss | Init Date                                                                                                                          | X              | NEO-LSE-BR-10005 | Stream event type title length can not be greater than 128          |
          | Feeling                                                                                                                           | ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss s | X              | NEO-LSE-BR-10006 | Stream event type metadata title length can not be greater than 128 |

    Scenario Outline: Stream event type is not allowed to get modified with duplicated matadata
        Given There is a defined stream event type with title with following properties
          | Title        | Metadata |
          | Conversation |          |
        And I have reprovided some stream event type metadata with following properties
          | Title    |
          | Duration |
          | Duration |
        When I modify stream event type 'Duration' with follwoing properties
          | Title        | Metadata |
          | Conversation |          |
        Then I get error with code 'NEO-LSE-BR-10007' and message 'There are duplicated metadata' within the system