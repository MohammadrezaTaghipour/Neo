Feature: Appending stream event

In order to registering and tracking life strams online
As me
I want to append an stream event

Scenario: Stream event gets appended with its valid properties
	Given There are some provided stream event type metadata with following properties
		| Title       |
		| Description |
		| HappendOn   |
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
	When I provide a new stream event with following properties
		| LifeStream             | StreamContext | StreamEventType |
		| Friendship with Souzan | Career Path   | Conversation    |
	And With the following stream event metadata
		| Key         | Value                                                              |
		| Description | We talked about the importance of Design Patterns in code quality. |
		| HappendOn   | 2022-01-01                                                         |
	And I append stream event 'Discussion with Souzan'
	Then I can find stream event 'Discussion with Souzan' with above properties