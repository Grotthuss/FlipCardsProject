import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './CardSetSelection.css';
import AddCardSet from './AddCardSet';

interface CardAttribute {
    Id: number;
    Question: string;
    Concept: string;
    Mnemonic: string;
}

interface CardSet {
    _set_name: string;
    _flipcards_list: CardAttribute[];
}

const CardSetSelection: React.FC = () => {
    const [cardSets, setCardSets] = useState<CardSet[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchCardSets = async () => {
            try {
                const response = await fetch('https://localhost:44372/api/Home');
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                setCardSets(data);
            } catch (error) {
                setError('Error fetching card sets: ' + (error as Error).message);
            } finally {
                setLoading(false);
            }
        };

        fetchCardSets();
    }, []);

    const handleAddCardSet = (setName: string) => {
        const newCardSet: CardSet = {
            _set_name: setName,
            _flipcards_list: [],
        };
        setCardSets([...cardSets, newCardSet]);
    };

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div className="card-set-selection-container">
            <div className="card-set-list">
                {Array.isArray(cardSets) && cardSets.length > 0 ? (
                    cardSets.map((cardSet, index) => (
                        <div key={index} className="card-set">
                            <Link to={`/card-set/${cardSet._set_name}`}>
                                <div className="card">
                                    <h2>{cardSet._set_name}</h2>
                                </div>
                            </Link>
                        </div>
                    ))
                ) : (
                    <div>No card sets available</div>
                )}
            </div>

            <div className="add-card-set-form">
                <AddCardSet onAdd={handleAddCardSet} />
            </div>
        </div>
    );
};

export default CardSetSelection;
