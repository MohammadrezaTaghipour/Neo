Feature: Removing life stream

In order to registering and tracking life strams online
As me
I want to configure life stream 

Scenario Outline: Life stream gets removed when no any events has registered for it
	Given There is a defined life stream with following properties
		| Title                  | Description |
		| Friendship with Souzan |             |
	When I remove life stream 'Friendship with Souzan'
	Then I can not find life stream 'Friendship with Souzan' with above properties

Scenario: Life stream is not allowed to get removed when stream any stream events has been appened for it
	Given There are some provided stream event type metadata with following properties
		| Title       |
		| Description |
		| HappenedOn  |
	And There is a defined stream event type with following properties
		| Title        | Metadata |
		| Conversation |          |
	And there is a provided stream context with following properties
		| Title       | Description                                             |
		| Career Path | this stream context includes all career-related stuffs. |
	And With following stream event types
		| StreamEventType |
		| Conversation    |
	And There is a defined stream context 'Career Path'
	And There is a defined life stream with following properties
		| Title                  | Description                         |
		| Friendship with Souzan | Our friendship started at July 2022 |
	And There is a provide stream event 'Conversation with Souzan' with following properties
		| LifeStream             | StreamContext | StreamEventType |
		| Friendship with Souzan | Career Path   | Conversation    |
	And With the following stream event metadata of stream event 'Conversation with Souzan'
		| Key         | Value                                                              |
		| Description | We talked about the importance of Design Patterns in code quality. |
		| HappenedOn  | 2022-01-01                                                         |
	And There is a appended stream event 'Conversation with Souzan'
	When I remove life stream 'Friendship with Souzan'
	Then I get error with code 'NEO-LS-BR-10006' and message 'Life stream cannot be removed because it has been used within the system' from the system