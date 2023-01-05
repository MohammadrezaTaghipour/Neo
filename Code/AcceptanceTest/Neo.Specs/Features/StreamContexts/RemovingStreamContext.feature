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
		| Conclusion   |          |
	And there is a provided stream context with following properties
		| Title       | Description |
		| Career path |             |
	And With following stream event types
		| StreamEventType |
		| Conversation    |
	And There is a defined stream context 'Career path'

Scenario: Stream context gets removed when no any events has registered for it
	When I remove stream context 'Career path'
	Then I can not find stream context 'Career path' with above properties