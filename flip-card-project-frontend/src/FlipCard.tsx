import React from 'react';
import './FlipCard.css';
import AddFlipCard from "./AddFlipcard";
import { useParams } from 'react-router-dom';
import {Errors} from "./errorEnums";

interface FlipCardData {
    Question: string;
    Concept: string;
    Mnemonic: string;
}

const FlipCard: React.FC = () => {
    const { title } = useParams<{ title: string }>();
    const [cards, setCards] = React.useState<FlipCardData[]>([]);
    const [loading, setLoading] = React.useState<boolean>(true);
    const [error, setError] = React.useState<string | null>(null);
    const [inputValue, setInputValue] = React.useState('');

    const flipAnsweredCard = (event: React.ChangeEvent<HTMLInputElement>) => {
        setInputValue(event.target.value);

        const updatedCards = [...cards];

        updatedCards.forEach((card, index) => {
            if (card.Concept.toLowerCase() === event.target.value.toLowerCase()) {
                handleFlip(index);
            }
        });
        setCards(updatedCards);
    };

    const handleFlip = (index: number) => {
        const flipCard = document.getElementById(`flipCard-${index}`);
        if (flipCard) {
            flipCard.classList.toggle('flipped');
        }
    };

    React.useEffect(() => {
        const fetchCards = async () => {
            if (!title) {
                setError(Errors.TITLE);
                setLoading(false);
                return;
            }
            try {
                const response = await fetch(`https://localhost:44372/api/Home/${title}`);
                if (!response.ok) {
                    throw new Error(Errors.NETWORK);
                }
                const data = await response.json();
                if (data && Array.isArray(data._flipcards_list)) {
                    setCards(data._flipcards_list);
                } else {
                    setError(Errors.FORMAT + JSON.stringify(data));
                }
            } catch (error) {
                setError(Errors.CARDS + (error as Error).message);
            } finally {
                setLoading(false);
            }
        };

        fetchCards();
    }, [title]);

    const handleAddFlipCard = async (question: string, concept: string, mnemonic: string) => {
        const newCard = {
            Question: question,
            Concept: concept,
            Mnemonic: mnemonic,
            state: {
                _state: 0,
            },
        };

        try {
            const response = await fetch(`https://localhost:44372/api/Home/${title}/CreateCard`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newCard),
            });

            if (!response.ok) {
                const errorMessage = await response.text();
                throw new Error(`${Errors.CARD} Server response: ${errorMessage}`);
            }

            setCards((prevCards) => [...prevCards, newCard]);
            setError(null);

        } catch (err) {
            setError(Errors.CARD + (err as Error).message);
        }
    };

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div className="flip-card-page">
            <div className="cards-container">
                {cards.length > 0 ? (
                    cards.map((card, index) => (
                        <div
                            className="flip-card"
                            key={index}
                            id={`flipCard-${index}`}
                            onClick={() => handleFlip(index)}
                        >
                            <div className="flip-card-inner">
                                <div className="flip-card-front">
                                    <p>{card.Question}</p>
                                    <h2>{card.Mnemonic}</h2>
                                </div>
                                <div className="flip-card-back">
                                    <h2>{card.Concept}</h2>
                                </div>
                            </div>
                        </div>
                    ))
                ) : (
                    <div>No cards available</div>
                )}
            </div>
            <div className="input-container">
                <input
                    type="text"
                    placeholder="Guess any concept..."
                    value={inputValue}
                    onChange={flipAnsweredCard}
                />
            </div>
            <AddFlipCard onAddFlipCard={handleAddFlipCard} />
        </div>
    );

};

export default FlipCard;
