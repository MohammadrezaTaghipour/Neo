Feature: Removing Stream Event

In order to appending and tracking life strams online
As me
I want to remove an stream event

Background:
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
	And There is a provide stream event 'Conversation with Souzan' with following properties
		| LifeStream             | StreamContext | StreamEventType |
		| Friendship with Souzan | Career Path   | Conversation    |
	And With the following stream event metadata of stream event 'Conversation with Souzan'
		| Key         | Value                                           |
		| Description | We talked about the future of software in 2025. |
		| HappenedOn  | 2022-01-02                                      |
	And There is a appended stream event 'Conversation with Souzan'

Scenario: Stream event gets removed properly
	When I remove stream event 'Conversation with Souzan'
	Then I can not find stream event 'Conversation with Souzan' with above properties
	
