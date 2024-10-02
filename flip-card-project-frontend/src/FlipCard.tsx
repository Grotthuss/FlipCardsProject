import React from 'react';

import './FlipCard.css';

/*
const FlipCard: React.FC = () => {
    const handleFlip = () => {
        const flipCard = document.getElementById('flipCard');
        if (flipCard) {
            flipCard.classList.toggle('flipped');
        }
    };

    return (
        <div className="container">
            <div className="flip-card" id="flipCard" onClick={handleFlip}>
                <div className="flip-card-inner">
                    <div className="flip-card-front">
                        <h2>Mnemonic</h2>
                    </div>
                    <div className="flip-card-back">
                        <h2>Concept</h2>
                    </div>
                </div>
            </div>
            <div className="input-container">
                <input
                    type="text"
                    id="textInput"
                    placeholder="Enter text here"
                />
                <button id="submitButton" onClick={handleFlip}>
                    Flip
                </button>
            </div>
        </div>
    );
};

export default FlipCard;*/

interface FlipCardData {
    id: number;
    concept: string;
    mnemonic: string;
}

const FlipCard: React.FC = () => {
    const [cards, setCards] = React.useState<FlipCardData[]>([]);
    const [loading, setLoading] = React.useState<boolean>(true);
    const [error, setError] = React.useState<string | null>(null);

    const handleFlip = (id: number) => {
        const flipCard = document.getElementById(`flipCard-${id}`);
        if (flipCard) {
            flipCard.classList.toggle('flipped');
        }
    };
    
    React.useEffect(() => {
        const fetchCards = async () => {
            try {
                const response = await fetch('https://localhost:44372/api/Home'); // Use the full URL
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                setCards(data);
            } catch (error) {
                setError('Error fetching cards: ' + (error as Error).message);
            } finally {
                setLoading(false);
            }
        };

        fetchCards();
    }, []);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        /*<div className="container">
            {cards.map((card) => (
                <div className="flip-card" key={card.id}>
                    <div className="flip-card-inner">
                        <div className="flip-card-front">
                            <h2>{card.mnemonic}</h2>
                        </div>
                        <div className="flip-card-back">
                            <h2>{card.concept}</h2>
                        </div>
                    </div>
                </div>
            ))}*/
        <div className="container">
            {cards.map((card) => (
                <div
                    className="flip-card"
                    key={card.id}
                    id={`flipCard-${card.id}`} // Give each card a unique ID
                    onClick={() => handleFlip(card.id)} // Add click handler
                >
                    <div className="flip-card-inner">
                        <div className="flip-card-front">
                            <h2>{card.mnemonic}</h2>
                        </div>
                        <div className="flip-card-back">
                            <h2>{card.concept}</h2>
                        </div>
                    </div>
                </div>
            ))}
            <div className="input-container">
                <input type="text" id="textInput" placeholder="Enter text here"/>
                {/*<button id="submitButton" onClick={() => handleFlip(`flipCard-${id}`)}>
                    Flip
                </button>*/}
            </div>

        </div>
    );
};

export default FlipCard;