Feature: Modifying life stream

In order to registering and tracking life strams online
As me
I want to configure life stream 

Scenario Outline: Life stream gets modified with its valid properties
	Given There is a defined life stream with following properties
		| Title                  | Description |
		| Friendship with Souzan |             |
	When I modify life stream 'Friendship with Souzan' with follwoing properties
		| Title               | Description                         |
		| Friendship with Ali | our friendship started at July 2022 |
	Then I can find life stream 'Friendship with Souzan' with above properties

Scenario Outline: Life stream is not allowed to get modified with invalid properties
	Given There is a defined life stream with following properties
		| Title                  | Description |
		| Friendship with Souzan |             |
	When I modify life stream 'Friendship with Souzan' with follwoing properties
		| Title   | Description   |
		| <title> | <description> |
	Then I get error with code '<errorCode>' and message '<errorMessage>' from the system

Examples:
	| errorCode       | errorMessage                                               | title                                                                                                                             | description                                                                                                                                                                                                                                                       |
	| NEO-LS-BR-10002 | Life stream title is required                              |                                                                                                                                   |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | $Friendship with Souzan                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | &Friendship with Souzan                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | @Friendship with Souzan                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | #Friendship with Souzan                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | !Friendship with Souzan                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | [Friendship with Souzan                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | Friendship with Souzan]                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | (Friendship with Souzan                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10003 | Life stream title can not include special characters       | Friendship with Souzan)                                                                                                           |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10004 | Life stream title length can not be greater than 128       | sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss |                                                                                                                                                                                                                                                                   |
	| NEO-LS-BR-10005 | Life stream description length can not be greater than 256 | X                                                                                                                                 | sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss |