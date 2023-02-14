Feature: Removing stream context

In order to registering and tracking life strams online
As me
I want to configure stream contexts

Background:
	Given There are some provided stream event type metadata with following properties
		| Title       |
		| Description |
		| HappenedOn  |
	And There are some defined stream event types with following properties
		| Title        | Metadata |
		| Conversation |          |
	And there is a provided stream context with following properties
		| Title       | Description |
		| Career Path |             |
	And With following stream event types
		| StreamEventType |
		| Conversation    |
	And There is a defined stream context 'Career Path'
	And There is a defined life stream with following properties
		| Title                  | Description                         |
		| Friendship with Souzan | Our friendship started at July 2022 |

Scenario: Stream context gets removed when no any events has registered for it
	When I remove stream context 'Career Path'
	Then I can not find stream context 'Career Path' with above properties

Scenario: Stream context is not allowed to get removed when an stream event is appended in
	When I provide a new stream event with following properties
		| LifeStream             | StreamContext | StreamEventType |
		| Friendship with Souzan | Career Path   | Conversation    |
	And With the following stream event metadata
		| Key         | Value                                                   |
		| Description | this stream context includes all career-related stuffs. |
		| HappenedOn  | 2022-01-01                                              |
	And I append stream event 'Conversation'
	When I remove stream context 'Career Path'
	Then I get error with code 'NEO-SC-BR-10009' and message 'Stream context can not be removed due to its usage' from the system
